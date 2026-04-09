using EShop.Domain.Enums;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Products.Commands;

public record UpdateProductStatusCommand(
    Guid ProductId,
    ProductStatus Status) : IRequest<bool>;

internal class UpdateProductStatusCommandHandler : IRequestHandler<UpdateProductStatusCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductStatusCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.FirstOrDefaultAsync(query);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        product.Status = request.Status;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class UpdateProductStatusCommandValidator : AbstractValidator<UpdateProductStatusCommand>
{
    public UpdateProductStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
        
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid product status.");
    }
}