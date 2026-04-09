using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class PaymentFailedEvent : IntegrationEvent
{
    public long OrderNumber { get; set; }
    public string TransactionId { get; set; }
}