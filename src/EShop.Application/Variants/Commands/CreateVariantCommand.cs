using EShop.Domain.Entities;
using EShop.Domain.Enums;
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

        var variant = Variant.Create(product.Id, product.Title ,request.Price, request.Quantity);

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