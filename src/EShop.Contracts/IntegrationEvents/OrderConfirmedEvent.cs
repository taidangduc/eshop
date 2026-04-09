using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class OrderConfirmedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}