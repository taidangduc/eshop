using System.Data;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Persistence;

public class EShopDbContext : DbContext, IUnitOfWork
{
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // 1. Use interceptors for Auditable Entities
        // 2. Save changes with concurrency handling

        try
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DBConcurrencyException("A concurrency conflict occurred while saving changes.", ex);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(EShopDbContext).Assembly);
    }
}
