namespace EShop.Application.Orders.DTOs;

public class OrderItemDto
{
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}