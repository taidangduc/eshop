using EShop.Api.Filters;
using EShop.Api.Models.Products;
using EShop.Application.Products.Commands;
using EShop.Application.Products.DTOs;
using EShop.Application.Products.Queries;
using EShop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
[Tags("Product")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductOverview>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetProductsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductModel request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.Name, request.Description, request.CategoryId);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetProduct), new { id = result }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductModel request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, request.Title, request.Description, request.CategoryId);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProductStatus(Guid id, [FromBody] ProductStatus status, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateProductStatusCommand(id, status), cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/images")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [FileValidation]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProductImage(Guid id, [FromForm] CreateProductImageModel request, CancellationToken cancellationToken)
    {
        var command = new CreateProductImageCommand(id, request.FormFile, request.IsMain);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{productId:guid}/images/{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProductImage(Guid productId, Guid imageId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductImageCommand(productId, imageId), cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/options")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductOption(Guid id, [FromBody] CreateProductOptionModel request, CancellationToken cancellationToken)
    {
        var command = new CreateProductOptionCommand(id, request.OptionName, request.HasImage);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetProduct), new { id }, result);
    }

    [HttpDelete("{productId:guid}/options/{optionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProductOption(Guid productId, Guid optionId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductOptionCommand(productId, optionId), cancellationToken);

        return NoContent();
    }

    [HttpPost("{productId:guid}/options/{optionId:guid}/values")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [FileValidation]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProductOptionValue(
        Guid productId,
        Guid optionId,
        [FromForm] CreateProductOptionValueModel request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductOptionValueCommand(productId, optionId, request.Value, request.MediaFile);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetProduct), new { id = productId }, result);
    }

    [HttpDelete("{productId:guid}/options/{optionId:guid}/values/{valueId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProductOptionValue(Guid productId, Guid optionId, Guid valueId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductOptionValueCommand(productId, optionId, valueId), cancellationToken);

        return NoContent();
    }
}
