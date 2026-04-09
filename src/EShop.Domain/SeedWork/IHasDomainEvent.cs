using EShop.Domain.Events;

namespace EShop.Domain.SeedWork;

public interface IHasDomainEvent
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent @event);
    void ClearDomainEvents();
}