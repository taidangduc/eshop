namespace EShop.Domain.Events;

public record OrderConfirmedDomainEvent(Guid OrderId) : IDomainEvent;
