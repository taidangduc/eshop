using EShop.Domain.Repositories;
using Ardalis.GuardClauses;
using EShop.Domain.Events;
using MediatR;
using System.Text.Json;
using EShop.Domain.Entities;
using EShop.Contracts.IntegrationEvents;

namespace EShop.Application.Orders.DomainEventHandlers;

public class OrderConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    IRepository<OutboxMessage, Guid> outboxRepository)
    : INotificationHandler<OrderConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IRepository<OutboxMessage, Guid> _outboxRepository = outboxRepository;

    public async Task Handle(OrderConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // Logic: payment succeeded -> set order status comfirmed / paid -> reservation stock (outbox)

        var order = await _orderRepository.GetAsync(domainEvent.OrderId);

        Guard.Against.NotFound(domainEvent.OrderId, order);

        var integrationEvent = new OrderConfirmedEvent { OrderId = order.Id };

        var message = new OutboxMessage
        {
            EventType = typeof(OrderConfirmedEvent).FullName,
            Payload = JsonSerializer.Serialize(integrationEvent),
        };

        await _outboxRepository.AddAsync(message);
        await _outboxRepository.UnitOfWork.SaveChangesAsync();
    }
}
