using EShop.Application.Variants.DTOs;
using EShop.Application.Variants.Services;
using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Queries;

public class VariantQueryService : IVariantQueryService
{
    private readonly EShopDbContext _dbContext;

    public VariantQueryService(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VariantDto> GetVariantAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        var variant = await _dbContext.Set<Variant>()
            .Include(v => v.OptionValues)
            .FirstOrDefaultAsync(v => v.Id == variantId, cancellationToken);

        return variant is not null ? MapToVariantDto(variant) : new();
    }

    public async Task<List<VariantDto>> GetVariantsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var variants = await _dbContext.Set<Variant>()
            .Include(v => v.OptionValues)
            .Where(v => ids.Contains(v.Id))
            .ToListAsync(cancellationToken);

        return variants.Count > 0 ? MapToVariantDtos(variants) : new();
    }

    /// <summary>
    /// [For case Match]:
    /// - MinPrice = MaxPrice
    /// - TotalStock = Quantity of the matched variant
    /// - Variants = the matched variant = 1
    /// <br/>
    /// [For case Not Match]:
    /// - MinPrice != MaxPrice
    /// - TotalStock = sum of the matched variants
    /// - Variants = the matched variants > 1
    /// </summary>
    public async Task<VariantOverview> GetVariantByOptionAsync(Guid productId, List<Guid> optionValueIds, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Variant>()
        .Include(v => v.OptionValues)
        .Where(v => v.ProductId == productId);

        if (optionValueIds.Count > 0)
        {
            query = query.Where(v => v.OptionValues.Count(ov => optionValueIds.Contains(ov.OptionValueId)) == optionValueIds.Count);
        }

        var variants = await query.ToListAsync(cancellationToken);

        return variants.Count > 0 ? MapToVariantOverview(variants) : new();
    }

    private VariantDto MapToVariantDto(Variant variant)
    {
        return new VariantDto
        {
            Id = variant.Id,
            ProductId = variant.ProductId,
            Title = variant.Title ?? string.Empty,
            Name = variant.Name ?? string.Empty,
            Sku = variant.Sku,
            Price = variant.Price,
            Quantity = variant.Quantity,
            ImageUrl = variant.ImageUrl ?? string.Empty,
            Options = variant.OptionValues.Select(ov => new VariantOptionDto
            {
                OptionValueId = ov.OptionValueId,
                OptionValueName = ov.OptionValueName ?? string.Empty,
            }).ToList()
        };
    }

    private List<VariantDto> MapToVariantDtos(List<Variant> variants)
    {
        return variants.Select(MapToVariantDto).ToList();
    }

    private VariantOverview MapToVariantOverview(List<Variant> variants)
    {
        return variants.Count > 0
        ? new VariantOverview
        {
            MinPrice = variants.Min(v => v.Price),
            MaxPrice = variants.Max(v => v.Price),
            TotalStock = variants.Sum(v => v.Quantity),
            Variants = MapToVariantDtos(variants)
        }
        : new VariantOverview();
    }
}
