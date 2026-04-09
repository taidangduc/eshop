using EShop.Application.Orders.DTOs;
using EShop.Domain.Repositories;

namespace EShop.Application.Orders.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetAsync(orderId, cancellationToken);

        return order == default ? null : DomainToDtoMapper.MapToOrderDto(order);
    }

    public async Task<OrderSummary> GetOrderSummaryAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetAsync(orderId, cancellationToken);

        return order == default ? null : DomainToDtoMapper.MapToOrderSummary(order);
    }
}