using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class GracePeriodEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}