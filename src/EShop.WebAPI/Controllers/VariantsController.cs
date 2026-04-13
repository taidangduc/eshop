using EShop.Api.Models.Variants;
using EShop.Application.Variants.Commands;
using EShop.Application.Variants.DTOs;
using EShop.Application.Variants.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/variants")]
[Tags("Variant")]
public class VariantsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VariantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVariantById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVariantByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost("by-options")]
    [ProducesResponseType(typeof(VariantOverview), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVariantByOptions([FromBody] GetVariantByOptionsModel request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVariantByOptionQuery(request.ProductId, request.OptionValueIds), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVariant([FromBody] CreateVariantModel request, CancellationToken cancellationToken)
    {
        var command = new CreateVariantCommand(request.ProductId, request.RegularPrice, request.Quantity);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetVariantById), new { id = result }, result);
    }

    [HttpPost("bulk")]
    [ProducesResponseType(typeof(List<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVariants(Guid productId, CancellationToken cancellationToken)
    {
        var command = new BulkCreateVariantCommand(productId);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVariant(Guid id, [FromBody] UpdateVariantModel request, CancellationToken cancellationToken)
    {
        var command = new UpdateVariantCommand(id, request.RegularPrice, request.Quantity);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("bulk")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVariants([FromBody] BulkUpdateVariantModel request, CancellationToken cancellationToken)
    {
        await mediator.Send(new BulkUpdateVariantCommand(request.ProductId, request.Price, request.Quantity, request.Sku), cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteVariant(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteVariantCommand(id), cancellationToken);

        return NoContent();
    }
}
