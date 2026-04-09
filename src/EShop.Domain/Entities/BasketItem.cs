namespace EShop.Domain.Entities;

public class BasketItem
{
    public Guid BasketId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
