namespace EShop.Application.Orders.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> Items { get; set; }
}