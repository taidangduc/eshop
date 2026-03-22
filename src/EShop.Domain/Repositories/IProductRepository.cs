using EShop.Domain.Entities;

namespace EShop.Domain.Repositories;

public interface IProductRepository : IRepository<Product, Guid>
{
}
