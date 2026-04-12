namespace EShop.Application.Orders.DTOs;

public class OrderStatusDto
{
    public Guid Id { get; set; }
    public long OrderNumber { get; set; }
    public Domain.Enums.OrderStatus Status { get; set; }
}