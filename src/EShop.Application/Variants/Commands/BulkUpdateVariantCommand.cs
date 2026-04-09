using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Variants.Commands;

public record BulkUpdateVariantCommand(
    Guid ProductId,
    decimal? Price = null,
    int? Quantity = null,
    string? Sku = null) : IRequest<bool>;

internal class UpdateVariantsCommandHandler : IRequestHandler<BulkUpdateVariantCommand, bool>
{
    private readonly IRepository<Variant, Guid> _variantRepository;

    public UpdateVariantsCommandHandler(IRepository<Variant, Guid> variantRepository)
    {
        _variantRepository = variantRepository;
    }

    public async Task<bool> Handle(BulkUpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var variants = await _variantRepository.GetQueryableSet()
            .Where(v => v.ProductId == request.ProductId)
            .ToListAsync(cancellationToken);

        if (variants.Count == 0)
            return true;

        foreach (var item in variants)
        {
            if (request.Price.HasValue)
            {
                item.Price = request.Price.Value;
            }

            if (request.Quantity.HasValue && item.Quantity != request.Quantity.Value && item.Quantity != 0)
            {
                item.Quantity = request.Quantity.Value;
            }

            if (request.Sku is not null)
            {
                item.Sku = request.Sku;
            }
        }

        return await _variantRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}