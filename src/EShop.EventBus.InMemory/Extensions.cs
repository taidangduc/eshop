using EShop.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;

namespace EShop.EventBus.InMemory;

public static class Extensions
{
    public static IEventBusBuilder AddInMemoryEventBus(this IHostApplicationBuilder builder)
    {
        var options = new InMemoryOptions();
        var channel = Channel.CreateBounded<MessageEnvelope>(options.MaxQueue);

        builder.Services.AddSingleton(channel);
        builder.Services.AddSingleton(options);

        builder.Services.AddSingleton<IEventPublisher, InMemorySender>();
        builder.Services.AddHostedService<InMemoryReceiver>();

        return new InMemoryEventBusBuilder(builder.Services);
    }
    private class InMemoryEventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}
