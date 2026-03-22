using EShop.Domain.Entities;
using EShop.Domain.Repositories;

namespace EShop.Persistence.Repositories;

public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
}
