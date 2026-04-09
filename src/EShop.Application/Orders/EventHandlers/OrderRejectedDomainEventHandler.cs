using EShop.Domain.Events;
using MediatR;

namespace EShop.Application.Orders.DomainEventHandlers;

public class OrderRejectedDomainEventHandler : INotificationHandler<OrderRejectedDomainEvent>
{
    public Task Handle(OrderRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Implement code reject order, ex: send notification, update stock

        return Task.CompletedTask;  
    }
}
