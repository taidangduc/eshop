using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Repositories;

public class OrderRepository : Repository<Order, Guid>, IOrderRepository
{
    private readonly EShopDbContext _dbContext;

    public OrderRepository(EShopDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Set<Order>().AddAsync(order);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Order>().FindAsync(id);
    }

    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Order>()
            .Include(m => m.Items)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Order>?> GetListByCustomerAsync(Guid customerId)
    {
        return await _dbContext.Set<Order>()
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Order?> GetByOrderNumber(long orderNumber)
    {
        return await _dbContext.Set<Order>()
            .SingleOrDefaultAsync(x => x.OrderNumber == orderNumber);
    }
    
    public async Task<List<Guid>> GetConfirmedGracePeriodOrdersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var cutoff = DateTime.UtcNow - TimeSpan.FromMinutes(15);

            var ids = await _dbContext.Set<Order>()
                .Where(x =>
                    x.OrderDate <= cutoff &&
                    x.Status == Domain.Enums.OrderStatus.Pending)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            return ids;
        }
        catch
        {
            Console.WriteLine("Error fetching confirmed grace period orders.");
        }

        return [];
    }
}
