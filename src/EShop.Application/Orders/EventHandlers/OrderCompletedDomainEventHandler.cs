using EShop.Domain.Events;
using MediatR;

namespace EShop.Application.Orders.DomainEventHandlers;

public class OrderCompletedDomainEventHandler : INotificationHandler<OrderCompletedDomainEvent>
{
    public Task Handle(OrderCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Implement code complete order, ex: send notification

        return Task.CompletedTask;
    }
}
