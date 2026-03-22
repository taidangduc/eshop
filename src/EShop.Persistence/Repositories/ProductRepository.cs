using EShop.Domain.Entities;
using EShop.Domain.Repositories;

namespace EShop.Persistence.Repositories;

public class ProductRepository : Repository<Product, Guid>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
}
