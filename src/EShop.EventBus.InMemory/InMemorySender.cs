using EShop.EventBus.Abstractions;
using EShop.EventBus.Events;
using System.Text.Json;
using System.Threading.Channels;

namespace EShop.EventBus.InMemory;

public class InMemorySender : IEventPublisher
{
    private readonly Channel<MessageEnvelope> _channel;

    public InMemorySender(Channel<MessageEnvelope> channel)
    {
        _channel = channel;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        var json = JsonSerializer.Serialize(@event, @event.GetType());

        var message = new MessageEnvelope(@event.GetType(), json);

        await _channel.Writer.WriteAsync(message);
    }
}
