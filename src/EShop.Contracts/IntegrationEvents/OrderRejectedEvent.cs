using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class OrderRejectedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}