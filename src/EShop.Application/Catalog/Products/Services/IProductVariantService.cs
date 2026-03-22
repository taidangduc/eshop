using EShop.Domain.Entities;

namespace EShop.Application.Catalog.Products.Services;

public interface IProductVariantService
{
    Task GenerateVariantsAsync(
        Product product, 
        Dictionary<Guid, List<Guid>> optionValues, 
        CancellationToken cancellationToken = default);
}
