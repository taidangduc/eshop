using EShop.Application.Basket.EventHandlers;
using EShop.Application.Catalog.Products.EventHandlers;
using EShop.Application.Customer.EventHandlers;
using EShop.Application.Order.IntegrationEventHandlers;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus;
using EShop.EventBus.Abstractions;
using EShop.EventBus.RabbitMQ;
using EShop.Infrastructure.ExternalServices.Payment;
using EShop.Infrastructure.ExternalServices.Payment.Vnpay;
using EShop.Infrastructure.ExternalServices.Payment.Stripe;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using EShop.Outbox.EF.Extensions;
using EShop.Outbox.EF.Infrastructure.Data;
using EShop.Persistence;
using EShop.Migrator;
using EShop.Application.Abstractions;

namespace EShop.Infrastructure;
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        // Get Configuration
        //var appSettings = builder.Configuration.GetOptions<AppSettings>();
        //builder.Services.Configure<VnpayOptions>(builder.Configuration.GetSection("VnpayConf"));
        //builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("StripeConf"));

        //builder.Services.AddSingleton(appSettings);

        // Add Persistence Layer
        //builder.AddPersistence();

        // External Services
        //builder.Services.AddTransient<IEmailService, SmtpEmailSender>();
        //builder.Services.AddScoped<IFileService, LocalStorage>();
               
        builder.Services.AddScoped<IPaymentGateway, VnpayPaymentGateway>();
        builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
        builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

        // register Vnpay implementation explicitly
        builder.Services.AddScoped<VnpayPaymentGateway>();

        // bind Stripe options and gateway
        builder.Services.AddScoped<StripePaymentGateway>();

        // Eventbus
        if (builder.Environment.EnvironmentName == "test")
        {
            builder.Services.AddTransient<IEventPublisher, NullEventPublisher>();
        }
        else
        {
            builder.AddRabbitMqEventBus("rabbitmq")
           .AddSubscription<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>()
           .AddSubscription<StockReservationRequestedIntegrationEvent, StockReservationRequestedIntegrationEventHandler>()
           .AddSubscription<GracePeriodConfirmedIntegrationEvent, GracePeriodConfirmedIntegrationEventHandler>()
           .AddSubscription<PaymentSucceededIntegrationEvent, PaymentSucceededIntegrationEventHandler>()
           .AddSubscription<PaymentRejectedIntegrationEvent, PaymentRejectedIntegrationEventHandler>()
           .AddSubscription<ReserveStockRejectedIntegrationEvent, ReserveStockRejectedIntegrationEventHandler>()
           .AddSubscription<ReserveStockSucceededIntegrationEvent, ReserveStockSucceededIntegrationEventHandler>()
           .AddSubscription<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>();
        }
        builder.AddTransactionalOutbox();     

        return builder;
    }

    public static WebApplication MapInfrastructure(this WebApplication app)
    {
        //app.UseCustomHealthCheck();

        return app;
    }

    public static async Task<WebApplication> MigrateAndSeedDataAsync(this WebApplication app)
    {
        await app.MigratePersistenceAsync();
        await app.MigrationDbContextAsync<OutboxDbContext>();

        return app;
    }
}
