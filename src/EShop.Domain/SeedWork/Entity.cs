namespace EShop.Domain.SeedWork;

public abstract class Entity : HasDomainEvent
{
    public Guid Id { get; set; }
}

public abstract class Entity<TKey> : Entity
{
    public new TKey Id { get; set; }
}