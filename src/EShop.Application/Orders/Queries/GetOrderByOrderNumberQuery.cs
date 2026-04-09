using EShop.Domain.Repositories;
using MediatR;
using EShop.Application.Orders.DTOs;

namespace EShop.Application.Orders.Queries;

public record GetOrderByOrderNumberQuery(long OrderNumber) : IRequest<CheckoutOrderDto>;

public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, CheckoutOrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByOrderNumberQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CheckoutOrderDto> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumber(request.OrderNumber);

        return order is not null ? MapToCheckoutOrderDto(order) : new();
    }

    private CheckoutOrderDto MapToCheckoutOrderDto(Domain.Entities.Order order)
    {
        return new CheckoutOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount.Amount,
            Status = order.Status,
            Currency = order.TotalAmount.Currency.ToString(),
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            PaymentMethod = order.PaymentMethod,
            PaymentProvider = order.PaymentProvider
        };
    }
}
