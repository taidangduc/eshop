namespace EShop.Domain.Events;

public record OrderCreatedDomainEvent(Guid OrderId, Guid CustomerId) : IDomainEvent;