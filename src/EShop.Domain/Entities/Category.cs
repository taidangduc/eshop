using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class Category : Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; }
}