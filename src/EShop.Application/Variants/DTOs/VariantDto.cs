namespace EShop.Application.Variants.DTOs;

public class VariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string? Sku { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public List<VariantOptionDto> Options { get; set; }
}