using Microsoft.Extensions.DependencyInjection;

namespace EShop.EventBus.RabbitMQ;


public static class Extensions
{
    public static IServiceCollection AddRabbitMQSender<T>(this IServiceCollection services, RabbitMQOptions options)
    {
        services.AddSingleton<IEventBusProducer<T>>(new RabbitMQSender<T>(new RabbitMQSenderOptions
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            ExchangeName = options.ExchangeName,
            ExchangeType = options.ExchangeType,
            RoutingKey = options.RoutingKeys[typeof(T).Name]
        }));

        return services;
    }

    public static IServiceCollection AddRabbitMQReceiver<TConsumer, T>(this IServiceCollection services, RabbitMQOptions options)
    {
        var receiverOptions = new RabbitMQReceiverOptions
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            ExchangeName = options.ExchangeName,
            ExchangeType = options.ExchangeType,
            RoutingKey = options.RoutingKeys[typeof(T).Name],
            QueueName = options.Consumers[typeof(TConsumer).Name][typeof(T).Name],
            AutomaticCreateEnabled = true,
        };

        services.AddSingleton<IEventBusConsumer<T>>(sp => new RabbitMQReceiver<T>(receiverOptions, sp.GetRequiredService<IEventDispatcher>()));

        return services;
    }
}
