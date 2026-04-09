namespace EShop.Domain.Entities;

public class OrderItem
{
    public Guid OrderId { get; set; }
    public Guid VariantId { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
    public string? ImageUrl { get; set; }
}
