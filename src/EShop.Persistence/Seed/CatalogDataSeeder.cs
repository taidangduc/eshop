using EShop.Domain.Entities;
using EShop.Migrator;

namespace EShop.Persistence.Seed;

public class CatalogDataSeeder : IDataSeeder<EShopDbContext>
{
    private readonly EShopDbContext _dbContext;

    public CatalogDataSeeder(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedCategories();
        await SeedProducts();
        // await SeedProductOptions();
        // await SeedProductOptionValues();
        // await SeedProductImages();
        // await SeedVariants();
        // await SeedVariantOptions();
    }

    private async Task SeedCategories()
    {
        if (!_dbContext.Set<Category>().Any())
        {
            await _dbContext.Set<Category>().AddRangeAsync(CatalogData.Categories);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SeedProducts()
    {
        if (!_dbContext.Set<Product>().Any())
        {
            await _dbContext.Set<Product>().AddRangeAsync(CatalogData.Products);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SeedProductOptions()
    {
        if (!_dbContext.Set<ProductOption>().Any())
        {
            await _dbContext.Set<ProductOption>().AddRangeAsync(CatalogData.ProductOptions);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SeedProductOptionValues()
    {
        if (!_dbContext.Set<ProductOptionValue>().Any())
        {
            await _dbContext.Set<ProductOptionValue>().AddRangeAsync(CatalogData.ProductOptionValues);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SeedProductImages()
    {
        if (!_dbContext.Set<ProductImage>().Any())
        {
            await _dbContext.Set<ProductImage>().AddRangeAsync(CatalogData.ProductImages);
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedVariants()
    {
        if (!_dbContext.Set<Variant>().Any())
        {
            await _dbContext.Set<Variant>().AddRangeAsync(CatalogData.Variants);
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedVariantOptions()
    {
        if (!_dbContext.Set<VariantOption>().Any())
        {
            await _dbContext.Set<VariantOption>().AddRangeAsync(CatalogData.VariantOptionValues);
            await _dbContext.SaveChangesAsync();
        }
    }
}
