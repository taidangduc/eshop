using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class OrderCancelledEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}