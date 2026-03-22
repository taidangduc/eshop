namespace EShop.Application.Catalog.Products.Queries.GetProductById;

public class ProductOptionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<OptionValueDto> Values { get; set; }
}
