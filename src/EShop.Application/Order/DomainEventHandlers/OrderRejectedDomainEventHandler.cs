using EShop.Domain.Events;
using MediatR;

namespace EShop.Application.Order.DomainEventHandlers;

public class OrderRejectedDomainEventHandler : INotificationHandler<OrderRejectedDomainEvent>
{
    public Task Handle(OrderRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        // implement code cancel order, ex: send notification, update stock

        return Task.CompletedTask;  
    }
}
