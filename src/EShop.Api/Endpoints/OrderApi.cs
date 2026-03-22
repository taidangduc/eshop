using EShop.Api.Models.Requests;
using EShop.Application.Order.Commands.CancelOrder;
using EShop.Application.Order.Commands.CreateOrder;
using EShop.Application.Order.Dtos;
using EShop.Application.Order.Queries.GetListOrder;
using EShop.Application.Order.Queries.GetOrderById;
using EShop.Application.Order.Queries.GetOrderByOrderNumber;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Endpoints;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("api/v1/orders")
            .MapOrderApi()
            .WithTags("Order Api");

        return builder;
    }

    public static RouteGroupBuilder MapOrderApi(this RouteGroupBuilder group)
    {
        group.MapGet("/",
            async(IMediator mediator, CancellationToken cancellationToken) =>
            {
                return await mediator.Send(new GetListOrderQuery(), cancellationToken);
            })
            .RequireAuthorization()
            .WithName("GetOrder")
            .WithSummary("Get order of current user")
            .Produces<List<OrderListDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet($"/{{id}}",
            async (IMediator mediator, Guid orderId, CancellationToken cancellationToken) =>
            {
                var command = new GetOrderByIdQuery(orderId);

                return await mediator.Send(command, cancellationToken);
            })
            .RequireAuthorization()
            .WithName("GetOrderById")
            .Produces<List<OrderItemDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet($"/checkout/{{orderNumber}}",
            async (IMediator mediator, long orderNumber, CancellationToken cancellationToken) =>
            {
                var query = new GetOrderByOrderNumberQuery(orderNumber);

                return await mediator.Send(query, cancellationToken);
            })
            .RequireAuthorization()
            .WithName("GetOrderByOrderNumber")
            .WithSummary("Get order by order number")
            .Produces<CheckoutOrderDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/",
            async (IMediator mediator, [FromBody] CreateOrderRequestDto request, CancellationToken cancellationToken) =>
            {
                var command = new CreateOrderCommand(request.CustomerId, request.Method, request.Provider, request.Street, request.City, request.ZipCode);

                var result = await mediator.Send(command, cancellationToken);

                return result;
            })
            .RequireAuthorization()
            .WithName("CreateOrder")
            .Produces<CreateOrderResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/",
            async (IMediator mediator, Guid orderId, CancellationToken cancellationToken) =>
            {
                var command = new CancelOrderCommand(orderId);

                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("CancelOrder")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}
