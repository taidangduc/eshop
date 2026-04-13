using EShop.Api.Models.Baskets;
using EShop.Application.Baskets.Commands;
using EShop.Application.Baskets.DTOs;
using EShop.Application.Baskets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/basket")]
[Authorize]
[Tags("Basket Api")]
public class BasketsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(BasketOverview), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBasket(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBasketQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketModel request, CancellationToken cancellationToken)
    {
        await mediator.Send(new AddOrUpdateBasketCommand(request.VariantId, request.Quantity), cancellationToken);

        return NoContent();
    }
}
