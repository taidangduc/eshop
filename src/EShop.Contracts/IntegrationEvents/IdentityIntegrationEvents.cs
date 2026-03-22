using EShop.EventBus.Events;

namespace EShop.Contracts.IntegrationEvents;

public class UserCreatedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
}
