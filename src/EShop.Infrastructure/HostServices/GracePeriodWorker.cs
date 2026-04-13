using EShop.Application.Orders.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop.Infrastructure.HostServices;

public class GracePeriodWorker : BackgroundService
{
    private readonly ILogger<GracePeriodWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GracePeriodWorker(ILogger<GracePeriodWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("GracePeriodWorker is starting.");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var publishGracePeriodCommand = new PublishGracePeriodCommand();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(publishGracePeriodCommand, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
            }

            await Task.Delay(10000, cancellationToken);
        }

        _logger.LogInformation("GracePeriodWorker is stopping.");
    }
}