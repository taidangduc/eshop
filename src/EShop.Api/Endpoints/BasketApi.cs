using EShop.Api.Models.Requests;
using EShop.Application.Basket.Commands.UpdateItem;
using EShop.Application.Basket.Dtos;
using EShop.Application.Basket.Queries.GetCartList;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Endpoints;

public static class BasketApi
{
    public static IEndpointRouteBuilder MapBasketApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/api/v1/basket")
            .MapBasketApi()
            .WithTags("Basket Api");
        return builder;
    }

    public static RouteGroupBuilder MapBasketApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            return await mediator.Send(new GetBasketQueryByCustomer());
        })
        .RequireAuthorization()
        .WithName("GetBasket")
        .WithSummary("Get basket of current user")
        .Produces<BasketDto>()
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/", async (IMediator mediator, [FromBody] UpdateBasketRequestDto request, CancellationToken cancellationToken) =>
        {
            await mediator.Send(new UpdateItemCommand(request.AccountId, request.VariantId, request.Quantity));

            return Results.NoContent();
        })
       .RequireAuthorization()
       .WithName("UpdateBasket")
       .WithSummary("Update basket of current user")
       .Produces<NoContent>()
       .ProducesProblem(StatusCodes.Status400BadRequest);

        return group; 
    }
}
