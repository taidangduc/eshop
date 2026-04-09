using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class StockDecreasedEvent : IntegrationEvent
{
   public Guid OrderId { get; set; }
}