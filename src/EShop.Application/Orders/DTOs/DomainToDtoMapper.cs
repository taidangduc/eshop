namespace EShop.Application.Orders.DTOs;

public static class DomainToDtoMapper
{
    public static OrderDto MapToOrderDto(Domain.Entities.Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Items = order.Items.Select(i => new OrderItemDto
            {
                VariantId = i.VariantId,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public static OrderSummary MapToOrderSummary(Domain.Entities.Order order)
    {
        return new OrderSummary
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount.Amount,
            OrderDate = order.OrderDate,
            Status = order.Status
        };
    }
}