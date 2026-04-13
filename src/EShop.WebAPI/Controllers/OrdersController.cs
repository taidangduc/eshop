using EShop.Api.Models.Order;
using EShop.Application.Orders.Commands;
using EShop.Application.Orders.DTOs;
using EShop.Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/orders")]
[Authorize]
[Tags("Order Api")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderOverview>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrdersQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(List<OrderItemOverview>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet("checkout/{orderNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrderCheckout(long orderNumber, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByOrderNumberQuery(orderNumber), cancellationToken);

        if (result == default)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderModel request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request.CustomerId, request.Method, request.Provider, request.Street, request.City, request.ZipCode);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelOrder(Guid orderId, CancellationToken cancellationToken)
    {
        await mediator.Send(new CancelOrderCommand(orderId), cancellationToken);

        return NoContent();
    }
}
