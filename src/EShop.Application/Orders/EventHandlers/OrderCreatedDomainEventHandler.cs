using EShop.Application.Baskets.Services;
using EShop.Domain.Events;
using MediatR;

namespace EShop.Application.Orders.DomainEventHandlers;

public class OrderCreatedDomainEventHandler(
    IBasketService basketService)
    : INotificationHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // Logic: orderCreated (integrationEvent) -> clearCart (integrationEventHandler)
        
        await basketService.ClearBasketAsync(domainEvent.CustomerId);
    }
}
