using EShop.Application.Baskets.Services;
using EShop.Application.Behaviors;
using EShop.Application.Variants.Services;
using EShop.Application.Common.Diagnotics.Commands;
using EShop.Application.Common.Diagnotics.Queries;
using EShop.Application.Common.Services;
using EShop.Application.Customers.Services;
using EShop.Application.FileEntries.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using EShop.Application.Orders.Services;

namespace EShop.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IVariantService, VariantService>();
        services.AddScoped<IFileEntryService, FileEntryService>();
        services.AddScoped<IOrderService, OrderService>();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceExtensions).Assembly);
        });

        // Monitoring
        services.AddSingleton<CommandHandlerMetrics>();
        services.AddSingleton<QueryHandlerMetrics>();
        services.AddSingleton<CommandHandlerActivity>();
        services.AddSingleton<QueryHandlerActivity>();

        // Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ActivityBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
