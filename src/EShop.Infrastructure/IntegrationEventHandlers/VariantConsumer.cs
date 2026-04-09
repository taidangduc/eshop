using System.Text.Json;
using EShop.Application.Orders.Services;
using EShop.Contracts.IntegrationEvents;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using EShop.EventBus;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.IntegrationEventHandlers;

public class VariantConsumer : IIntegrationEventHandler<OrderConfirmedEvent>
{
    private readonly IRepository<Variant, Guid> _variantRepository;
    private readonly IRepository<OutboxMessage, Guid> _outboxMessageRepository;
    private readonly IOrderService _orderService;

    public VariantConsumer(
        IRepository<Variant, Guid> variantRepository,
        IRepository<OutboxMessage, Guid> outboxMessageRepository,
        IOrderService orderService)
    {
        _variantRepository = variantRepository;
        _outboxMessageRepository = outboxMessageRepository;
        _orderService = orderService;
    }
    public async Task HandleAsync(OrderConfirmedEvent message, CancellationToken cancellationToken = default)
    {
        var orderItems = await _orderService.GetOrderAsync(message.OrderId, cancellationToken);

        var variantIds = orderItems.Items.Select(i => i.VariantId).ToList();

        var response = await _variantRepository.GetQueryableSet().Where(v => variantIds.Contains(v.Id)).ToListAsync(cancellationToken);

        var variantByIds = response?.ToDictionary(v => v.Id) ?? [];

        foreach (var item in orderItems.Items)
        {
            if (!variantByIds.TryGetValue(item.VariantId, out var variant) || variant.Quantity < item.Quantity)
            {
                var failedEvent = new StockDecreaseFailedEvent { OrderId = message.OrderId };

                await _outboxMessageRepository.AddAsync(new OutboxMessage
                {
                    EventType = typeof(StockDecreaseFailedEvent).FullName,
                    Payload = JsonSerializer.Serialize(failedEvent),
                });

                await _outboxMessageRepository.UnitOfWork.SaveChangesAsync();

                return;
            }

            variant.ReserveStock(item.Quantity);
        }

        var decreasedEvent = new StockDecreasedEvent { OrderId = message.OrderId };

        await _outboxMessageRepository.AddAsync(new OutboxMessage
        {
            EventType = typeof(StockDecreasedEvent).FullName,
            Payload = JsonSerializer.Serialize(decreasedEvent),
        });

        await _outboxMessageRepository.UnitOfWork.SaveChangesAsync();

        await _variantRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}