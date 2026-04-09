namespace EShop.Application.Products.DTOs;

public class ProductOverview
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    /// <summary>
    /// The price of the product, which is the minimum price among its variants.
    /// </summary>
    public decimal Price { get; set; }
    public string? Thumbnail { get; set; }
}