using Microsoft.EntityFrameworkCore;

namespace EShop.Migrator;

public interface IDataSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(CancellationToken cancellationToken);
}
