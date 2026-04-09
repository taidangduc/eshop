namespace EShop.Domain.SeedWork;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long Version { get; set; }
}

public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long Version { get; set; }
}