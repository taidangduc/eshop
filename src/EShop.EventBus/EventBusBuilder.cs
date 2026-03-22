using Microsoft.Extensions.DependencyInjection;

namespace EShop.EventBus;

public interface IEventBusBuilder
{
    IServiceCollection Services { get; }
}

public class EventBusBuilder : IEventBusBuilder
{
    public IServiceCollection Services { get; }
    public EventBusBuilder(IServiceCollection services)
    {
        Services = services;
    }    
}
