using EShop.Domain.Repositories;
using MediatR;
using EShop.Application.Orders.DTOs;

namespace EShop.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<List<OrderOverview>>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, List<OrderOverview>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderOverview>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        return order is not null ? MapToOrderOverview(order) : new();

    }

    private List<OrderOverview> MapToOrderOverview(Domain.Entities.Order order)
    {
        return new List<OrderOverview>
        {
            new OrderOverview
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
            }
        };
    }
}
