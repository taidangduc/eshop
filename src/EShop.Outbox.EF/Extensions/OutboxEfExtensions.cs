using EShop.EventBus.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EShop.Outbox.Abstractions;
using EShop.Outbox.EF.Infrastructure;
using EShop.Outbox.EF.Infrastructure.Data;
using EShop.Outbox.EF.Infrastructure.Services;

namespace EShop.Outbox.EF.Extensions;

public static class OutboxEfExtensions
{
    public static void AddTransactionalOutbox(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<OutboxDbContext>("shopdb");

        builder.Services.AddScoped<IOutboxMessageProcessor, OutboxMessageProcessor>();
        builder.Services.AddScoped<IPollingOutboxMessageRepository, PollingOutboxMessageRepository>();
        builder.Services.AddSingleton<PollingOutboxMessageRepositoryOptions>();

        // In-memory event bus for demo/testing purposes
        // In production, consider using a more robust event bus like RabbitMQ, Azure Service Bus, etc.
        builder.AddInMemoryEventBus();

        // Hosted service to poll and process outbox messages
        builder.Services.AddHostedService<TransactionalOutboxPollingService>();
    }
}

