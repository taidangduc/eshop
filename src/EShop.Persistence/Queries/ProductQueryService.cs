using EShop.Application.Products.DTOs;
using EShop.Application.Products.Services;
using EShop.Application.Variants.DTOs;
using EShop.Domain.Entities;
using EShop.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Queries;

public class ProductQueryService : IProductQueryService
{
    private readonly EShopDbContext _dbContext;

    public ProductQueryService(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get product details by product id, including basic info, variant summary, gallery and options
    /// I used left join to get the product basic info, so that it can return null if the product is not found, instead of throwing exception
    /// You can create a view / stored procedure / read model in database to simplify the query
    /// </summary>
    public async Task<ProductDetail> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        // PRODUCT BASIC INFO
        var product = await (
            from p in _dbContext.Set<Product>()
            join c in _dbContext.Set<Category>() on p.CategoryId equals c.Id into pc
            from c in pc.DefaultIfEmpty()
            where p.Id == productId && !p.IsDeleted && p.Status == ProductStatus.Published
            select new
            {
                p.Id,
                p.Title,
                p.Description,
                p.CategoryId,
                CategoryName = c != null ? c.Name : string.Empty
            }).FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            return new();
        }

        // NOTE: all for one
        // if a variant with option, all variants of the product should have option,  // VARIANT SUMMARY
        // if a variant without option, just a variant of the product, and without option

        // VARIANT SUMMARY
        var variantSummary = await (
            from v in _dbContext.Set<Variant>()
            where v.ProductId == productId
            group v by v.ProductId into g
            select new VariantSummary
            {
                // default variant id when variant without option
                // if a variant with option, check condition HasOption
                VariantId = g.OrderBy(v => v.Price).Select(v => v.Id).FirstOrDefault(),
                HasOption = g.Any(v => v.OptionValues.Any()),
                MinPrice = g.Min(x => x.Price),
                MaxPrice = g.Max(x => x.Price),
                TotalStock = g.Sum(x => x.Quantity)
            }).FirstOrDefaultAsync(cancellationToken);

        // PRODUCT IMAGES
        var gallery = await (
            from i in _dbContext.Set<ProductImage>()
            where i.ProductId == productId
            orderby i.SortOrder
            select new ProductImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                SortOrder = i.SortOrder
            }).ToListAsync(cancellationToken);

        // PRODUCT OPTIONS
        var options = await (
            from o in _dbContext.Set<ProductOption>()
            where o.ProductId == productId
            select new ProductOptionDto
            {
                Id = o.Id,
                Name = o.Name,
                Values = o.Values.Select(ov => new ProductOptionValueDto
                {
                    Id = ov.Id,
                    Name = ov.Name,
                    ImageUrl = ov.ImageUrl ?? string.Empty
                }).ToList()
            }).ToListAsync(cancellationToken);

        return new ProductDetail
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = product.CategoryName,
            VariantSummary = variantSummary ?? new(),
            Gallery = gallery ?? new(),
            Options = options ?? new()
        };
    }

    // ref: https://stackoverflow.com/questions/3404975/left-outer-join-in-linq
    // ref: https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/join-operations#emulate-a-left-outer-join
    public async Task<List<ProductOverview>> GetProductsAsync(CancellationToken cancellationToken = default)
    {

        var productOveriew =
            from p in _dbContext.Set<Product>()
            where !p.IsDeleted && p.Status == ProductStatus.Published
            let minPrice = _dbContext.Set<Variant>()
                .Where(v => v.ProductId == p.Id)
                .Select(v => (decimal?)v.Price)
                .Min()
            // If you want to filter out products without variants, 
            // you can add the following condition:
            // where minPrice != null
            select new ProductOverview
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CategoryId = p.CategoryId,
                Price = minPrice ?? 0,

                Thumbnail = p.Images.Where(x => x.IsMain).Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty
            };

        return await productOveriew.ToListAsync(cancellationToken);
    }
}
