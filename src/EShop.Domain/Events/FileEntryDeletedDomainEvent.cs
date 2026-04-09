namespace EShop.Domain.Events;

public record FileEntryDeletedDomainEvent(string? FileLocation) : IDomainEvent;