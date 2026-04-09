using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record SetCompletedOrderStatusCommand(Guid OrderId) : IRequest<bool>;

internal class SetCompletedOrderStatusCommandHandler : IRequestHandler<SetCompletedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetCompletedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetCompletedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order is null)
        {
            return false;
        }

        order.SetCompletedStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class SetCompletedOrderStatusCommandValidator : AbstractValidator<SetCompletedOrderStatusCommand>
{
    public SetCompletedOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");
    }
}
