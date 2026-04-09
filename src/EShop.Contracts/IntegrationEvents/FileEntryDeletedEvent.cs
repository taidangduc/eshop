using EShop.EventBus;

namespace EShop.Contracts.IntegrationEvents;

public class FileEntryDeletedEvent : IntegrationEvent
{
    public string? FileLocation { get; set; }
}