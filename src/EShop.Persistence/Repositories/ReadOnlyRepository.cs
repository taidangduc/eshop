using EShop.Domain.Repositories;
using EShop.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Repositories;

public class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    private readonly EShopDbContext _dbContext;

    public ReadOnlyRepository(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

    public IQueryable<TEntity> GetQueryableSet()
    {
        return _dbContext.Set<TEntity>();
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return query.ToListAsync();
    }
}
