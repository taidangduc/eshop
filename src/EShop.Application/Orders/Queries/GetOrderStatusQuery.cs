using EShop.Application.Orders.DTOs;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Orders.Queries;

public record GetOrderStatusQuery(Guid OrderId) : IRequest<OrderStatusDto>;

internal class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderStatusQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderStatusDto> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        return order is not null ? DomainToDtoMapper.MapToOrderStatusDto(order) : new();
    }
}