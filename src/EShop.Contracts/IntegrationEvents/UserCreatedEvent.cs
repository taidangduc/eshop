using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class UserCreatedEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
}