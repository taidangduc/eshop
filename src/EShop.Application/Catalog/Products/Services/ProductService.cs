using EShop.Application.Abstractions;
using EShop.Application.Common.Services;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;

namespace EShop.Application.Catalog.Products.Services;

public class ProductService : CrudService<Product>, IProductService
{
    public ProductService(IRepository<Product, Guid> productRepository, ICurrentUserProvider currentUser)
        : base(productRepository)
    {

    }
}
