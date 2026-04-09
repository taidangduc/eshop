namespace EShop.Application.Baskets.DTOs;

public class BasketItemOverview
{
    public Guid VariantId { get; set; }
    public string Title { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
}