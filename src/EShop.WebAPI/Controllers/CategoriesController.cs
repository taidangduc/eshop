using EShop.Api.Models.Categories;
using EShop.Application.Categories.Commands;
using EShop.Application.Categories.DTOs;
using EShop.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
[Tags("Category")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCategoriesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Title);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetCategories), new { id = result }, result);
    }
}
