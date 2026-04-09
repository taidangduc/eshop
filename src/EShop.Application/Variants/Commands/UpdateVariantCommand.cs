using EShop.Domain.Entities;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Variants.Commands;

public record UpdateVariantCommand(
    Guid VariantId,
    decimal? Price = null,
    int? Quantity = null,
    string? Sku = null) : IRequest<bool>;

internal class UpdateVariantCommandHandler(
    IRepository<Variant, Guid> variantRepository) : IRequestHandler<UpdateVariantCommand, bool>
{
    public async Task<bool> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var query = variantRepository.GetQueryableSet()
            .Where(v => v.Id == request.VariantId);

        var variant = await variantRepository.FirstOrDefaultAsync(query);

        if (variant is null)
        {
            throw new NotFoundException($"Variant with ID {request.VariantId} not found.");
        }

        if (request.Price.HasValue)
        {
            variant.Price = request.Price.Value;
        }

        if (request.Quantity.HasValue)
        {
            variant.Quantity = request.Quantity.Value;
        }

        if (request.Sku is not null)
        {
            variant.Sku = request.Sku;
        }

        await variantRepository.UpdateAsync(variant, cancellationToken);

        return await variantRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class UpdateVariantCommandValidator : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty().WithMessage("VariantId is required.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.")
            .When(x => x.Price.HasValue);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative.")
            .When(x => x.Quantity.HasValue);
    }
}