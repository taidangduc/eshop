using EShop.Domain.Entities;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Variants.Commands;

public record CreateVariantCommand(
    Guid ProductId,
    decimal Price,
    int Quantity) : IRequest<Guid>;

/// <summary>
/// Create variant no option, just for simple product, 
/// if the product has options, should use bulk create 
/// </summary>
internal class CreateVariantCommandHandler(
    IRepository<Variant, Guid> variantRepository, IRepository<Product, Guid> productRepository) : IRequestHandler<CreateVariantCommand, Guid>
{
    public async Task<Guid> Handle(CreateVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetQueryableSet()
            .Where(p => p.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }
        // NOTE: condition
        // avoid many variant no option, just a variant no option
        // variant of a product, just have two cases:
        // 1./ a variant no option
        // 2./ many variant with option
        // => just choose a optional
        // NOTE: all for one
        // if a variant with option, all variants of the product should have option, 
        // if a variant without option, just a variant of the product, and without option

        var existingVariant = await variantRepository.GetQueryableSet()
            .Where(v => v.ProductId == request.ProductId)
            .Include(vo => vo.OptionValues)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingVariant != null && !existingVariant.OptionValues.Any())
        {
            throw new InvalidOperationException("A variant for this product already exists.");
        }

        if (existingVariant != null && existingVariant.OptionValues.Any())
        {
            throw new InvalidOperationException("Cannot create a variant without options for a product that already has variants with options.");
        }

        var variant = Variant.Create(product.Id, product.Title, request.Price, request.Quantity);

        await variantRepository.AddAsync(variant, cancellationToken);
        await variantRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return variant.Id;
    }
}

public class CreateVariantCommandValidator : AbstractValidator<CreateVariantCommand>
{
    public CreateVariantCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative.");
    }
}