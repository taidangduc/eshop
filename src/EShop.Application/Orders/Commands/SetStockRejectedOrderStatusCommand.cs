using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record SetStockRejectedOrderStatusCommand(Guid OrderId) : IRequest<bool>;

internal class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return false;
        }

        order.SetRejectedStatusWhenStockRejected();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class SetStockRejectedOrderStatusCommandValidator : AbstractValidator<SetStockRejectedOrderStatusCommand>
{
    public SetStockRejectedOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");
    }
}
