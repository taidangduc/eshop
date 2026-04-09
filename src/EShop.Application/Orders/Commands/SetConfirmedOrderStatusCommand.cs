using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record SetConfirmedOrderStatusCommand(
    long OrderNumber,
    string? CardBrand,
    string? TransactionId) : IRequest<bool>;

internal class SetConfirmedOrderStatusCommandHandler : IRequestHandler<SetConfirmedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumber(command.OrderNumber);

        if (order is null)
        {
            return false;
        }

        order.CardBrand = command.CardBrand;
        order.TransactionId = command.TransactionId;

        order.SetConfirmedStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class SetConfirmedOrderStatusCommandValidator : AbstractValidator<SetConfirmedOrderStatusCommand>
{
    public SetConfirmedOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderNumber)
            .GreaterThan(0).WithMessage("OrderNumber must be greater than 0.");
    }
}
