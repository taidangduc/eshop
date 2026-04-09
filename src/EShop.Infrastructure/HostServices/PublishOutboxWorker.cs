using EShop.Application.OutboxMessages.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop.Infrastructure.HostServices;

public class PublishOutboxWorker : BackgroundService
{
    private readonly ILogger<PublishOutboxWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public PublishOutboxWorker(ILogger<PublishOutboxWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PublishOutboxWorker is starting.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new PublishOutboxMessageCommand(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing outbox messages.");
            }

            await Task.Delay(5000, cancellationToken);
        }

        _logger.LogInformation("PublishOutboxWorker is stopping.");
    }
}