using System.Text.Json;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using EShop.EventBus;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Application.OutboxMessages.Commands;

public record PublishOutboxMessageCommand() : IRequest<Unit>;

internal class PublishOutboxMessageCommandHandler : IRequestHandler<PublishOutboxMessageCommand, Unit>
{
    private readonly IRepository<OutboxMessage, Guid> _outboxMessageRepository;
    private readonly IEventBus _eventBus;
    private readonly ILogger<PublishOutboxMessageCommandHandler> _logger;

    public PublishOutboxMessageCommandHandler(
        IRepository<OutboxMessage, Guid> outboxMessageRepository,
        IEventBus eventBus,
        ILogger<PublishOutboxMessageCommandHandler> logger)
    {
        _outboxMessageRepository = outboxMessageRepository;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<Unit> Handle(PublishOutboxMessageCommand request, CancellationToken cancellationToken)
    {
        var events = _outboxMessageRepository.GetQueryableSet()
            .Where(x => !x.Published && x.ScheduledAt <= DateTime.UtcNow)
            .OrderBy(x => x.ScheduledAt)
            .Take(50)
            .ToList();

        foreach (var outbox in events)
        {
            var integrationEvent = DeserializeToContractType(outbox);

            if (integrationEvent == null)
            {
                continue;
            }

            await _eventBus.SendAsync((dynamic)integrationEvent, cancellationToken);

            outbox.Published = true;
            await _outboxMessageRepository.UpdateAsync(outbox, cancellationToken);
        }

        await _outboxMessageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    // Case 1:
    // If you have the assembly qualified name, you can directly get the type and deserialize
    private IntegrationEvent? DeserializeToIntegrationEvent(OutboxMessage message)
    {
        if (string.IsNullOrEmpty(message.EventType))
        {
            _logger.LogWarning("Event type is null or empty for message {MessageId}", message.Id);
            return null;
        }

        var eventType = Type.GetType(message.EventType, throwOnError: false);

        if (eventType == null || !typeof(IntegrationEvent).IsAssignableFrom(eventType))
        {
            _logger.LogWarning("Could not find event type {EventType} for message {MessageId}", message.EventType, message.Id);
            return null;
        }

        return JsonSerializer.Deserialize(message.Payload, eventType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) as IntegrationEvent;
    }

    // Case 2: 
    // If you only have the full name, you need to search for the type in the current domain assemblies
    // Not recommended if you need performance, as it can be expensive to search through all assemblies and types.
    public static Type? GetFirstMatchingTypeFromCurrentDomainAssembly(string typeName)
    {
        var result = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))
            .FirstOrDefault();

        return result;
    }

    private IntegrationEvent? DeserializeJsonContent(OutboxMessage message)
    {
        var type = GetFirstMatchingTypeFromCurrentDomainAssembly(message.EventType);

        if (type == null)
        {
            _logger.LogWarning("Failed to find type {EventType}", message.EventType);
            return null;
        }

        return JsonSerializer.Deserialize(message.Payload, type) as IntegrationEvent;
    }

    // Case 3: 
    // Create Contracts project and reference it in both producer and consumer,
    // Then you can directly deserialize to the contract type.
    // This is the recommended approach for better maintainability and decoupling.
    private IntegrationEvent? DeserializeToContractType(OutboxMessage message)
    { 
        var eventType = typeof(EShop.Contracts.AssemblyReference).Assembly.GetType(message.EventType);
        if (eventType == null)
        {
            _logger.LogWarning("Failed to find type {EventType}", message.EventType);
            return null;
        }

        return JsonSerializer.Deserialize(message.Payload, eventType) as IntegrationEvent;
    }
}
