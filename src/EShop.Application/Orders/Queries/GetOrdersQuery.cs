using EShop.Domain.Repositories;
using MediatR;
using EShop.Application.Orders.DTOs;
using EShop.Application.Customers.Queries;
using EShop.Domain.Identity;

namespace EShop.Application.Orders.Queries;

public record GetOrdersQuery() : IRequest<List<OrderOverview>>;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderOverview>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUser _currentWebUser;
    private readonly IMediator _mediator;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        ICurrentUser currentWebUser,
        IMediator mediator)
    {
        _orderRepository = orderRepository;
        _currentWebUser = currentWebUser;
        _mediator = mediator;
    }

    public async Task<List<OrderOverview>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {

        var customerId = await _mediator.Send(new GetCustomerQuery(_currentWebUser.UserId));

        if (customerId is null)
        {
            throw new Exception("Not found customer");
        }

        var orders = await _orderRepository.GetListByCustomerAsync(customerId.Id);

        return orders is not null ? MapToOrderOverview(orders) : new();
    }

    private List<OrderOverview> MapToOrderOverview(List<Domain.Entities.Order> orders)
    {
        return orders.Select(order => new OrderOverview
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount.Amount,
            Status = order.Status,
            OrderDate = order.OrderDate,
            Items = order.Items.Select(i => new OrderItemOverview
            {
                VariantId = i.VariantId,
                Name = i.Name,
                Title = i.Title,
                Quantity = i.Quantity,
                UnitPrice = i.Price,
                ImageUrl = i.ImageUrl
            }).ToList()
        }).ToList();
    }
}
