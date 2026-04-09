namespace EShop.Application.Orders.DTOs;

public class OrderItemOverview
{
    public Guid OrderId { get; set; }
    public Guid VariantId { get; set; }
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public string ImageUrl { get; set; }
}
