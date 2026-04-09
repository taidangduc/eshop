using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class Basket : AuditableEntity<Guid>, IAggregateRoot
{
    public Guid CustomerId { get; set; }
    public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

    public void ClearItems() 
    { 
        Items.Clear();
    }
}
