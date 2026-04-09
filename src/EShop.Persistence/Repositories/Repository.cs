using EShop.Domain.Repositories;
using EShop.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>, IAggregateRoot
{
    private readonly EShopDbContext _dbContext;
    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

    public IUnitOfWork UnitOfWork => _dbContext;

    public Repository(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default(TKey)))
        {
            return AddAsync(entity, cancellationToken);
        }
        else
        {
            return UpdateAsync(entity, cancellationToken);
        }
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        //_dbContext.Update(entity).State = EntityState.Modified;

        return Task.CompletedTask;
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public IQueryable<TEntity> GetQueryableSet()
    {
        return _dbContext.Set<TEntity>();
    }

    public Task<T1> FirstOrDefaultAsync<T1>(IQueryable<T1> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T1> SingleOrDefaultAsync<T1>(IQueryable<T1> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T1>> ToListAsync<T1>(IQueryable<T1> query)
    {
        return query.ToListAsync();
    }
}
