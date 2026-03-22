using EShop.Domain.Events;
using MediatR;

namespace EShop.Application.Order.DomainEventHandlers;

public class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    public Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        // implement code cancel order, ex: send notification, update stock

        return Task.CompletedTask;
    }
}
