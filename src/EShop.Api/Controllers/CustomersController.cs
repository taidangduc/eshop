using EShop.Application.Customers.DTOs;
using EShop.Application.Customers.Queries;
using EShop.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/customers")]
[Authorize]
[Tags("Customer Api")]
public class CustomersController(IMediator mediator, ICurrentUser currentWebUser) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCustomer(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCustomerQuery(currentWebUser.UserId), cancellationToken);

        return Ok(result);
    }
}
