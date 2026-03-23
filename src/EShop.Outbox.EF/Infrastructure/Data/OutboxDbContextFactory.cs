using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EShop.Outbox.EF.Infrastructure.Data;

public class OutboxDbContextFactory : IDesignTimeDbContextFactory<OutboxDbContext>
{
    public OutboxDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OutboxDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=EShopDb;Username=postgres;Password=postgres;");

        return new OutboxDbContext(optionsBuilder.Options);
    }
}
