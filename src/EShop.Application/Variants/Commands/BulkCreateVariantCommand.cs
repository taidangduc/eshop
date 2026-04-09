using EShop.Application.Variants.DTOs;
using EShop.Domain.Entities;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Variants.Commands;

public record BulkCreateVariantCommand(Guid ProductId) : IRequest<bool>;

internal class CreateVariantsCommandHandler : IRequestHandler<BulkCreateVariantCommand, bool>
{
    private readonly IRepository<Variant, Guid> _variantRepository;
    private readonly IRepository<Product, Guid> _productRepository;

    public CreateVariantsCommandHandler(IRepository<Variant, Guid> variantRepository, IRepository<Product, Guid> productRepository)
    {
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(BulkCreateVariantCommand request, CancellationToken cancellationToken)
    {
        // get option values for the product to validate the variant options
        var product = await _productRepository.GetQueryableSet()
            .Include(p => p.Options)
                .ThenInclude(o => o.Values)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        var optionList = product.Options
            .Select(x => x.Values
                .Select(y => new VariantDimension
                {
                    OptionId = x.Id,
                    OptionName = x.Name,
                    HasImage = x.HasImage,
                    OptionValueId = y.Id,
                    OptionValueName = y.Name,
                    ImageUrl = y.ImageUrl
                }).ToList()
            ).ToList();


        // descartes/cross join
        var cartesianProduct = GetCartesianProduct(optionList);

        foreach (var combination in cartesianProduct)
        {
            var variant = new Variant
            {
                // don't need to set SKU for auto-generated variants,
                // client can update SKU later with UpdateVariantCommand
                ProductId = request.ProductId,
                Title = product.Title + " - " + GetCombineName(combination),
                Price = 0,
                Quantity = 0,
                ImageUrl = combination.FirstOrDefault(v => v.HasImage)?.ImageUrl,
                Status = VariantStatus.Active,
                Name = GetCombineName(combination),
            };

            variant.SetOptionValues(
                combination.Select(c => new VariantOption
                {
                    OptionId = c.OptionId,
                    OptionName = c.OptionName,
                    OptionValueId = c.OptionValueId,
                    OptionValueName = c.OptionValueName
                }).ToList());

            await _variantRepository.AddAsync(variant, cancellationToken);
        }

        await _variantRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private List<List<VariantDimension>> GetCartesianProduct(List<List<VariantDimension>> sequences)
    {
        IEnumerable<IEnumerable<VariantDimension>> cartesianProduct = new[] { Enumerable.Empty<VariantDimension>() };

        foreach (var sequence in sequences)
        {
            cartesianProduct = from accseq in cartesianProduct
                               from item in sequence
                               select accseq.Concat(new[] { item });
        }

        return cartesianProduct.Select(seq => seq.ToList()).ToList();
    }

    private string GetCombineName(List<VariantDimension> optionValues)
    {
        return string.Join(" / ", optionValues.Select(v => v.OptionValueName));
    }
}

public class CreateVariantsCommandValidator : AbstractValidator<BulkCreateVariantCommand>
{
    public CreateVariantsCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
    }
}