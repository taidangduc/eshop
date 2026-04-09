using EShop.Application.Variants.DTOs;

namespace EShop.Application.Variants.Services;

/// <summary>
/// External service, use via abstraction, return ViewModel or Dto
/// Internal service, use directly, return entity or Dto
/// </summary>
public class VariantService : IVariantService
{
    private readonly IVariantQueryService _variantQueryService;

    public VariantService(IVariantQueryService variantQueryService)
    {
        _variantQueryService = variantQueryService;
    }

    public async Task<VariantDto> GetVariantAsync(Guid VariantId, CancellationToken cancellationToken = default)
    {
        var variant = await _variantQueryService.GetVariantAsync(VariantId, cancellationToken);
        return variant;
    }

    public async Task<List<VariantDto>> GetVariantsByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken = default)
    {
        var variants = await _variantQueryService.GetVariantsByIdsAsync(Ids, cancellationToken);
        return variants;
    }
}