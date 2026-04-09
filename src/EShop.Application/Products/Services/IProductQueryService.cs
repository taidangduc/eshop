using EShop.Application.Products.DTOs;

namespace EShop.Application.Products.Services;

public interface IProductQueryService
{
    /// <summary>
    /// Get product details by product id,
    /// including basic info, gallery, options and variants.
    /// </summary>
    Task<ProductDetail> GetProductAsync(Guid productId, CancellationToken cancellationToken = default);
    /// <summary>
    /// [For case user]
    /// Get a list of product overviews. 
    /// Each overview includes basic info and the minimum price among its variants.
    /// </summary>
    Task<List<ProductOverview>> GetProductsAsync(CancellationToken cancellationToken = default);
}