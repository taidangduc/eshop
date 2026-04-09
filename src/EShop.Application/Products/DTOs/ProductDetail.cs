using EShop.Application.Variants.DTOs;

namespace EShop.Application.Products.DTOs;

public class ProductDetail
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public VariantSummary VariantSummary { get; set; }
    public List<ProductImageDto> Gallery { get; set; }
    public List<ProductOptionDto> Options { get; set; }
}