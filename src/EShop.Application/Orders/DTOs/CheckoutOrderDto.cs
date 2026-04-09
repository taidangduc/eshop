using EShop.Domain.Enums;

namespace EShop.Application.Orders.DTOs;

public class CheckoutOrderDto
{
    public Guid Id { get; set; }
    public long OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid CustomerId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentProvider PaymentProvider { get; set; }
}
