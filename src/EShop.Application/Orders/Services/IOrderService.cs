using EShop.Application.Orders.DTOs;

namespace EShop.Application.Orders.Services;

public interface IOrderService
{
    Task<OrderDto> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<OrderSummary> GetOrderSummaryAsync(Guid orderId, CancellationToken cancellationToken = default);
}