using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class StockDecreaseFailedEvent : IntegrationEvent
{
   public Guid OrderId { get; set; }
}