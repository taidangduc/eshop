using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class OutboxMessage : Entity<Guid>, IAggregateRoot
{
    public string Payload { get; set; }
    public string EventType { get; set; }
    public bool Published { get; set; }
    public DateTime ScheduledAt { get; set; } = DateTime.UtcNow;
}