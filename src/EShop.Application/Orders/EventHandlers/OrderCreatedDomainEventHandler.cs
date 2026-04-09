using EShop.Contracts.IntegrationEvents;
using EShop.Domain.Entities;
using EShop.Domain.Events;
using EShop.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace EShop.Application.Orders.DomainEventHandlers;

public class OrderCreatedDomainEventHandler(
    IRepository<OutboxMessage, Guid> outboxRepository)
    : INotificationHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // Logic: order created (integrationEvent) -> clear cart (integrationEventHandler)

        var integrationEvent = new OrderCreatedEvent
        {
            OrderId = domainEvent.OrderId,
            CustomerId = domainEvent.CustomerId,
        };

        var message = new OutboxMessage
        {
            EventType = typeof(OrderCreatedEvent).FullName,
            Payload = JsonSerializer.Serialize(integrationEvent),
        };

        await outboxRepository.AddAsync(message);
        await outboxRepository.UnitOfWork.SaveChangesAsync();
    }
}
