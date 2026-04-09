using EShop.Domain.SeedWork;

namespace EShop.Domain.Repositories;

public interface IReadOnlyRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    IQueryable<TEntity> GetQueryableSet();
    Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);
    Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query);
    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
}
