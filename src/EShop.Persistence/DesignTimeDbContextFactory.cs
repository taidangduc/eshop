using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EShop.Persistence;

public class EShopDesignTimeDbContextFactory : IDesignTimeDbContextFactory<EShopDbContext>
{
    public EShopDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EShopDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=EShopDb;Username=postgres;Password=postgres");

        return new EShopDbContext(optionsBuilder.Options);
    }
}
