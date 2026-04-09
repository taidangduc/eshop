using EShop.Application.FileEntries.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Products.Commands;

public record CreateProductImageCommand(
    Guid ProductId,
    IFormFile File,
    bool IsMain = false) : IRequest<bool>;

internal class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileEntryService _fileEntryService;

    public CreateProductImageCommandHandler(
        IProductRepository productRepository,
        IFileEntryService fileEntryService)
    {
        _productRepository = productRepository;
        _fileEntryService = fileEntryService;
    }

    public async Task<bool> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .Include(p => p.Images)
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.FirstOrDefaultAsync(query);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        var fileEntry = await _fileEntryService.CreateFileEntryAsync(request.File, cancellationToken);

        var sortOrder = product.Images.Any() ? product.Images.Max(i => i.SortOrder) + 1 : 0;

        product.AddImage(request.IsMain, sortOrder, fileEntry.FileLocation);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class CreateProductImageCommandValidator : AbstractValidator<CreateProductImageCommand>
{
    public CreateProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
           .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.File)
           .NotNull().WithMessage("File is required.");

        RuleFor(x => x.File.ContentType)
           .Must(ct => ct.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
           .WithMessage("File must be an image type.")
           .When(x => x.File is not null);

        RuleFor(x => x.File.Length)
           .GreaterThan(0).WithMessage("File must not be empty.")
           .When(x => x.File is not null);
    }
}