using EShop.Application.FileEntries.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Products.Commands;

public record CreateProductOptionValueCommand(
    Guid ProductId,
    Guid OptionId,
    string Value,
    IFormFile? File = null) : IRequest<bool>;

internal class CreateProductOptionValueCommandHandler : IRequestHandler<CreateProductOptionValueCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileEntryService _fileEntryService;

    public CreateProductOptionValueCommandHandler(
        IProductRepository productRepository,
        IFileEntryService fileEntryService)
    {
        _productRepository = productRepository;
        _fileEntryService = fileEntryService;
    }

    public async Task<bool> Handle(CreateProductOptionValueCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .AsQueryable()
            .Include(p => p.Options)
                .ThenInclude(o => o.Values)
            .Where(p => p.Id == request.ProductId);

        var product = await query.FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        string? fileLocation = null;

        if (request.File is not null)
        {
            var fileEntry = await _fileEntryService.CreateFileEntryAsync(request.File, cancellationToken);

            fileLocation = fileEntry.FileLocation;
        }

        product.AddOptionValue(request.OptionId, request.Value, fileLocation);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class CreateProductOptionValueCommandValidator : AbstractValidator<CreateProductOptionValueCommand>
{
    public CreateProductOptionValueCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("OptionId is required.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required.")
            .MaximumLength(255).WithMessage("Value must not exceed 255 characters.");
    }
}