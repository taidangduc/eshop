namespace EShop.Application.Products.DTOs;

public class ProductImageDto
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}
