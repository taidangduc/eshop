using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class PaymentSucceedEvent : IntegrationEvent
{
    public long OrderNumber { get; set; }
    public string TransactionId { get; set; }
    public string? CardBrand { get; set; }
}