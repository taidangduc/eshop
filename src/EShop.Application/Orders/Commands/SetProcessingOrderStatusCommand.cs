using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record SetProcessingOrderStatusCommand(Guid OrderId) : IRequest<bool>;

internal class SetProcessingOrderStatusCommandHandler : IRequestHandler<SetProcessingOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetProcessingOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetProcessingOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return false;
        }

        order.SetProcessingStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class SetProcessingOrderStatusCommandValidator : AbstractValidator<SetProcessingOrderStatusCommand>
{
    public SetProcessingOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");
    }
}
