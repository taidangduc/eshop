using EShop.Domain.Entities;

namespace EShop.Domain.Repositories;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message);
    Task<List<OutboxMessage>> GetPendingMessagesAsync();
    Task MarkAsProcessedAsync(Guid messageId);
}

public class PublishOutboxMessage
{
    
}