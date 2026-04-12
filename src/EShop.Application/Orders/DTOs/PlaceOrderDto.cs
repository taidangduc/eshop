namespace EShop.Application.Orders.DTOs;

public class PlaceOrderDto
{
    public Guid OrderId { get; set; }
    public long OrderNumber { get; set; }
    public string? PaymentUrl { get; set; }
}