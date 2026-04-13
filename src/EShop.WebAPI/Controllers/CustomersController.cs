using EShop.Application.Customers.Commands;
using EShop.Application.Customers.Queries;
using EShop.Contracts.Customer.DTOs;
using EShop.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/customers")]
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

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerModel request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCustomerCommand(request.UserId, request.Email), cancellationToken);

        return Ok(result);
    }
}
