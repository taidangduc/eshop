using EShop.Application.Products.Services;
using EShop.Application.Variants.Services;
using EShop.Domain.Repositories;
using EShop.Migrator;
using EShop.Persistence.Interceptors;
using EShop.Persistence.Queries;
using EShop.Persistence.Repositories;
using EShop.Persistence.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        // Interceptors
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        // Single DbContext for both reads and writes
        services.AddDbContext<EShopDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, EShopDbContext>();

        // Repositories
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Query services
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IVariantQueryService, VariantQueryService>();

        // Seeders
        services.AddScoped<IDataSeeder<EShopDbContext>, CatalogDataSeeder>();

        return services;
    }

    public static async Task<WebApplication> MigratePersistenceAsync(this WebApplication app)
    {
        await app.MigrationDbContextAsync<EShopDbContext>();

        return app;
    }
}
