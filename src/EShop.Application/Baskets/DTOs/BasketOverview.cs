namespace EShop.Application.Baskets.DTOs;

public class BasketOverview
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<BasketItemOverview> Items { get; set; } = new();
};