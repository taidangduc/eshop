using EShop.Application.Customer.Commands;
using Ardalis.GuardClauses;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus.Abstractions;
using MediatR;

namespace EShop.Application.Customer.EventHandlers;

public class UserCreatedIntegrationEventHandler(IMediator mediator) : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task Handle(UserCreatedIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        await mediator.Send(new CreateCustomerCommand(integrationEvent.UserId, integrationEvent.Email));
    }
}
