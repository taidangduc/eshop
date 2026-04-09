using EShop.Domain.Entities;

namespace EShop.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, Guid>
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Order>?> GetListByCustomerAsync(Guid customerId);
    Task<Order?> GetByOrderNumber(long orderNumber);
    Task AddAsync(Order order);
    Task<List<Guid>> GetConfirmedGracePeriodOrdersAsync(CancellationToken cancellationToken = default);
}
