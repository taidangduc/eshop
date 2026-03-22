using EShop.Application.Common.Dtos;

namespace EShop.Application.Catalog.Products.Queries.GetProductById;

public class OptionValueDto
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public ImageLookupDto? Image { get; set; }
}
