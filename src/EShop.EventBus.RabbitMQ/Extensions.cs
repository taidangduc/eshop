using EShop.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShop.EventBus.RabbitMQ;

public static class Extensions
{
    public static IEventBusBuilder AddRabbitMqEventBus(this IHostApplicationBuilder builder, string connectionName = "rabbitmq")
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddRabbitMQClient(connectionName);

        builder.Services.AddSingleton<IEventPublisher, RabbitMqSender>();
        builder.Services.AddHostedService<RabbitMqReceiver>();

        return new RabbitMqEventBusBuilder(builder.Services);
    }

    private class RabbitMqEventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}

