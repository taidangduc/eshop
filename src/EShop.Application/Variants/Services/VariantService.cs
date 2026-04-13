using EShop.Application.Orders.Services;
using EShop.Application.Variants.DTOs;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Variants.Services;

/// <summary>
/// External service, use via abstraction, return ViewModel or Dto
/// Internal service, use directly, return entity or Dto
/// </summary>
public class VariantService : IVariantService
{
    private readonly IVariantQueryService _variantQueryService;
    private readonly IRepository<Variant, Guid> _variantRepository;
    private readonly IOrderService _orderService;

    public VariantService(IVariantQueryService variantQueryService, IRepository<Variant, Guid> variantRepository, IOrderService orderService)
    {
        _variantQueryService = variantQueryService;
        _variantRepository = variantRepository;
        _orderService = orderService;
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

    public async Task<bool> ReserveStockAsync(Guid OrderId, CancellationToken cancellationToken = default)
    {
        var orderItems = await _orderService.GetOrderAsync(OrderId, cancellationToken);

        var variantIds = orderItems.Items.Select(i => i.VariantId).ToList();

        var response = await _variantRepository.GetQueryableSet().Where(v => variantIds.Contains(v.Id)).ToListAsync(cancellationToken);

        var variantByIds = response?.ToDictionary(v => v.Id) ?? [];

        foreach (var item in orderItems.Items)
        {
            if (!variantByIds.TryGetValue(item.VariantId, out var variant) || variant.Quantity < item.Quantity)
            {
                return false;
            }

            variant.ReserveStock(item.Quantity);
        }

        await _variantRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return true;
    }
}