using EShop.Api.Models.Payment;
using EShop.Application.Payments.Commands;
using EShop.Application.Payments.Services;
using EShop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/v1/payments")]
[Authorize]
[Tags("Payment Api")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    // NOTE: 
    // You should create payment url while in transaction of creating order
    // OPTIONAL: 
    // Create payment url in a separate endpoint just for demo purpose
    [HttpPost]
    [ProducesResponseType(typeof(CreatePaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentUrlModel request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreatePaymentCommand(request.OrderId, request.Provider), cancellationToken);

        return Ok(result);
    }

    // OPTIONAL: 
    // This endpoint is not required by payment providers,
    // it's just for demo purpose to show how to handle return url without coupling with webhook handling logic.
    [HttpGet("return/{provider}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaymentResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleReturn([FromRoute] PaymentProvider provider, CancellationToken cancellationToken)
    {
        var parameters = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

        var result = await mediator.Send(new HandleReturnCommand(provider, parameters), cancellationToken);

        return Ok(result);
    }

    // NOTE: 
    // Webhook endpoint is required by payment providers to receive asynchronous payment result notifications.
    // This is the [SOURCE OF TRUTH] for payment result.
    [HttpPost("webhook/{provider}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleWebhook([FromRoute] PaymentProvider provider, CancellationToken cancellationToken)
    {
        // Retrieve raw body
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync(cancellationToken);

        // Retrieve raw headers
        var headers = Request.Headers.ToDictionary(
            x => x.Key,
            x => x.Value.ToString());

        var result = await mediator.Send(new HandleWebhookCommand(provider, body, headers), cancellationToken);

        return result ? Ok() : BadRequest();
    }
}
