using EShop.Contracts.IntegrationEvents;
using EShop.Domain.Repositories;
using EShop.EventBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop.Infrastructure.HostServices;

public class GracePeriodWorker(
    ILogger<GracePeriodWorker> logger,
    IOrderRepository orderRepository,
    IEventBus eventBus
    ) : BackgroundService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IEventBus _eventBus = eventBus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //var delayTime = TimeSpan.FromSeconds(_options.CheckUpdateTime);

        logger.LogInformation("GracePeriodWorker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckConfirmedGracePeriodOrders(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }

        logger.LogInformation("GracePeriodWorker is stopping");
    }

    private async Task CheckConfirmedGracePeriodOrders(CancellationToken cancellationToken)
    {
        var orderIds = await _orderRepository.GetConfirmedGracePeriodOrdersAsync(cancellationToken);
        foreach (var orderId in orderIds)
        {
            var integrationEvent = new GracePeriodEvent { OrderId = orderId };

            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", integrationEvent.OrderId, integrationEvent);

            await _eventBus.SendAsync(integrationEvent, cancellationToken);
        }
    }
}