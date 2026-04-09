using EShop.Application.FileEntries.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Products.Commands;

public record DeleteProductImageCommand(
    Guid ProductId,
    Guid ImageId) : IRequest<bool>;

internal class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductImageCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .Include(p => p.Images)
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.FirstOrDefaultAsync(query);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        product.RemoveImage(request.ImageId);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class DeleteProductImageCommandValidator : AbstractValidator<DeleteProductImageCommand>
{
    public DeleteProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.ImageId)
            .NotEmpty().WithMessage("ImageId is required.");
    }
}