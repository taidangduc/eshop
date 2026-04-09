using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.EventBus;

public class EventBus : IEventBus, IEventDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static List<Type> _consumers = new List<Type>();
    internal static void AddConsumers(Assembly assembly, IServiceCollection services)
    {
        var types = assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)))
            .ToList();

        foreach (var type in types)
        {
            services.AddTransient(type);
        }

        _consumers.AddRange(types);
    }


    public EventBus(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
       where T : EventBusMessage
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IEventBusProducer<T>>().SendAsync(message, cancellationToken);
    }

    public async Task DispatchAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        foreach (Type handlerType in _consumers)
        {
            // Case 1: strongly typed dispatching
            // if (_serviceProvider.GetService(handlerType) is IIntegrationEventHandler<T> handler)
            // {
            //     await handler.HandleAsync(message, cancellationToken);
            // }

            // Case 2: reflection based dispatching
            bool canHandleEvent = handlerType.GetInterfaces()
                .Any(i => i.IsGenericType
                && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)
                && i.GetGenericArguments()[0] == typeof(T));

            if (canHandleEvent)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                dynamic consumer = scope.ServiceProvider.GetService(handlerType);
                await consumer.HandleAsync((dynamic)message, cancellationToken);
            }
            else
            {
                Console.WriteLine($"No handler found for message type {typeof(T).Name} in consumer {handlerType.Name}");
            }
        }
    }
}

public static class EventBusExtensions
{
    public static void AddEventBusConsumers(this IServiceCollection services, Assembly assembly)
    {
        EventBus.AddConsumers(assembly, services);
    }

    public static void AddEventBus(this IServiceCollection services, Assembly assembly)
    {
        services.AddEventBusConsumers(assembly);
        services.AddTransient<IEventBus, EventBus>();
        services.AddTransient<IEventDispatcher, EventBus>();
    }
}