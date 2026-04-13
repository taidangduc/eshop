using EShop.Application.Variants.DTOs;

namespace EShop.Application.Variants.Services;

/// <summary>
/// Shared service, use DTO to return data
/// </summary>
public interface IVariantService
{
    Task<VariantDto> GetVariantAsync(Guid VariantId, CancellationToken cancellationToken = default);
    Task<List<VariantDto>> GetVariantsByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken = default);
    Task<bool> ReserveStockAsync(Guid OrderId, CancellationToken cancellationToken = default);
}