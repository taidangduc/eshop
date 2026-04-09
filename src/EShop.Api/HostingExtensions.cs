using EShop.Api.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using EShop.ServiceDefaults.OpenApi;
using EShop.EventBus;
using EShop.EventBus.RabbitMQ;
using EShop.Contracts.IntegrationEvents;
using EShop.Api.ConfigurationOptions;
using EShop.Infrastructure.IntegrationEventHandlers;
using System.Reflection;
using EShop.Infrastructure.HostServices;
using EShop.Infrastructure.Identity;

namespace EShop.Api;

public static class HostingExtensions
{
    public static IServiceCollection AddHosting(this IServiceCollection services, AppSettings appSettings)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins", builder =>
            {
                builder.WithOrigins(appSettings.CORS.AllowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        //services.ConfigureApplicationCookie(options =>
        //{
        //    options.Cookie.SameSite = SameSiteMode.Lax;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //});

        //services.Configure<CookiePolicyOptions>(options =>
        //{
        //    options.MinimumSameSitePolicy = SameSiteMode.Lax;
        //    options.Secure = CookieSecurePolicy.SameAsRequest;
        //});

        services.AddJwt();

        //services.AddCustomHealthCheck();

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddDefaultOpenApi();

        services.AddHttpContextAccessor();

        // Data Protection-keys: cookie auth, session, identity, antiforgery => persist key, encryptor
        //services.AddCustomDataProtection();

        services.AddIdentity();

        services.AddEventBus(typeof(Program).Assembly);
        services.AddEventBusConsumers(Assembly.GetAssembly(typeof(CustomerConsumer)));
        services.AddTransient<IEventBus, EventBus.EventBus>();
        services.AddTransient<IEventDispatcher, EventBus.EventBus>();
        services.AddRabbitMQSender<OrderCreatedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<OrderConfirmedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<OrderCompletedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<OrderCancelledEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<PaymentSucceedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<PaymentFailedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<StockDecreasedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<StockDecreaseFailedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQSender<GracePeriodEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<BasketConsumer, OrderCreatedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<CustomerConsumer, UserCreatedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<VariantConsumer, OrderConfirmedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<OrderConsumer, StockDecreasedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<OrderConsumer, StockDecreaseFailedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<OrderConsumer, PaymentSucceedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<OrderConsumer, PaymentFailedEvent>(appSettings.RabbitMQ);
        services.AddRabbitMQReceiver<OrderConsumer, GracePeriodEvent>(appSettings.RabbitMQ);

        AddHostedServices(services);

        return services;
    }

    public static WebApplication MapHosting(this WebApplication app)
    {
        app.UseForwardedHeaders();

        app.UseStaticFiles();

        app.UseCors("AllowedOrigins");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapDefaultEndpoints();

        app.MapControllers();

        if (app.Environment.IsEnvironment("Docker") || app.Environment.IsDevelopment())
        {
            app.UseDefaultOpenApi();
        }

        return app;
    }

    static void AddHostedServices(IServiceCollection services)
    {
        services.AddHostedService<EventBusBackgroundService<CustomerConsumer, UserCreatedEvent>>();
        services.AddHostedService<EventBusBackgroundService<VariantConsumer, OrderConfirmedEvent>>();
        services.AddHostedService<EventBusBackgroundService<BasketConsumer, OrderCreatedEvent>>();
        services.AddHostedService<EventBusBackgroundService<OrderConsumer, StockDecreasedEvent>>();
        services.AddHostedService<EventBusBackgroundService<OrderConsumer, StockDecreaseFailedEvent>>();
        services.AddHostedService<EventBusBackgroundService<OrderConsumer, PaymentSucceedEvent>>();
        services.AddHostedService<EventBusBackgroundService<OrderConsumer, PaymentFailedEvent>>();
        services.AddHostedService<EventBusBackgroundService<OrderConsumer, GracePeriodEvent>>();

        // services.AddHostedService<GracePeriodWorker>();
        services.AddHostedService<PublishOutboxWorker>();
    }
}
