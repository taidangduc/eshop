using EShop.Application.Variants.DTOs;

namespace EShop.Application.Variants.Services;

public interface IVariantQueryService
{
    Task<VariantDto> GetVariantAsync(Guid VariantId, CancellationToken cancellationToken = default);
    Task<List<VariantDto>> GetVariantsByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken = default);
    Task<VariantOverview> GetVariantByOptionAsync(Guid ProductId, List<Guid> OptionValueIds, CancellationToken cancellationToken = default);
}