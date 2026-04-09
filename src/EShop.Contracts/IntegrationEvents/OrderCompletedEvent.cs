using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class OrderCompletedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}