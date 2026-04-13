using EShop.Domain.Repositories;
using EShop.Domain.Events;
using MediatR;
using EShop.Application.Variants.Services;

namespace EShop.Application.Orders.DomainEventHandlers;

public class OrderConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    IVariantService variantService)
    : INotificationHandler<OrderConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IVariantService _variantService = variantService;

    public async Task Handle(OrderConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // Logic: payment succeeded -> set order status comfirmed / paid -> reservation stock (outbox)
        
        var order = await _orderRepository.GetAsync(domainEvent.OrderId);

        if (order is null)
        {
            return;
        }

        var response = await _variantService.ReserveStockAsync(domainEvent.OrderId, cancellationToken);

        if(response == true)
        {
            order.SetConfirmedStatus();
        }
        else
        {
            order.SetRejectedStatusWhenStockRejected();
        }

    }
}
