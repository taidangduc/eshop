namespace EShop.Application.Products.DTOs;

public class ProductOptionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ProductOptionValueDto>? Values { get; set; }
}