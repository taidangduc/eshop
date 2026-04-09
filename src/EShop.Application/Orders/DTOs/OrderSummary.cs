using EShop.Domain.Enums;

namespace EShop.Application.Orders.DTOs;

public class OrderSummary
{
    public Guid Id { get; set; }
    public long OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
}