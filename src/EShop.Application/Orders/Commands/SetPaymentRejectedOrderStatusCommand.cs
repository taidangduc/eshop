using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record SetPaymentRejectedOrderStatusCommand(long OrderNumber) : IRequest<bool>;

internal class SetPaymentRejectedOrderStatusCommandHandler : IRequestHandler<SetPaymentRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetPaymentRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetPaymentRejectedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumber(command.OrderNumber);

        if (order is null)
        {
            return false;
        }

        order.SetRejectedStatusWhenPaymentRejected();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class SetPaymentRejectedOrderStatusCommandValidator : AbstractValidator<SetPaymentRejectedOrderStatusCommand>
{
    public SetPaymentRejectedOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderNumber)
            .GreaterThan(0).WithMessage("OrderNumber must be greater than 0.");
    }
}
