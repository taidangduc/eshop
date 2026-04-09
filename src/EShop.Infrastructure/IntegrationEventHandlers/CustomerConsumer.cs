using EShop.Application.Customers.Commands;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus;
using MediatR;

namespace EShop.Infrastructure.IntegrationEventHandlers;

public class CustomerConsumer : IIntegrationEventHandler<UserCreatedEvent>
{
    private readonly IMediator _mediator;

    public CustomerConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task HandleAsync(UserCreatedEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new CreateCustomerCommand(message.UserId, message.Email), cancellationToken);
    }
}