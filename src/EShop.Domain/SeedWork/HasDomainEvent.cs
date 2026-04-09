using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EShop.Domain.Events;

namespace EShop.Domain.SeedWork;

public abstract class HasDomainEvent : IHasDomainEvent
{
    private List<IDomainEvent> _domainEvents = new();
    
    [JsonIgnore]
    [NotMapped]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
