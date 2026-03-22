namespace EShop.Domain.Events;

public record OrderRejectedDomainEvent(Guid OrderId) : IDomainEvent;
