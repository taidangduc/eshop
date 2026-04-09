using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
}