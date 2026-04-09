using System.Text.Json;
using EShop.Application.Orders.Services;
using EShop.Application.Payments.Services;
using EShop.Contracts.IntegrationEvents;
using EShop.Domain.Entities;
using EShop.Domain.Enums;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Payments.Commands;

public record HandleWebhookCommand(
    PaymentProvider provider,
    IDictionary<string, string> parameters)
    : IRequest<PaymentResultResponse>;

internal class HandleWebhookCommandHandler : IRequestHandler<HandleWebhookCommand, PaymentResultResponse>
{
    private readonly IPaymentGatewayFactory _factory;
    private readonly IOrderService _orderService;
    private readonly IRepository<OutboxMessage, Guid> _outboxRepository;

    public HandleWebhookCommandHandler(IPaymentGatewayFactory factory, IOrderService orderService, IRepository<OutboxMessage, Guid> outboxRepository)
    {
        _factory = factory;
        _orderService = orderService;
        _outboxRepository = outboxRepository;
    }

    // [Source of truth]
    public async Task<PaymentResultResponse> Handle(HandleWebhookCommand request, CancellationToken cancellationToken)
    {
        var gateway = _factory.Resolve(request.provider);
        var response = await gateway.HandleWebhookAsync(request.parameters);

        if (response.IsSuccess)
        {
            var paymentSucceed = new PaymentSucceedEvent
            {
                OrderNumber = response.OrderNumber,
                TransactionId = response.TransactionId,
            };

            await _outboxRepository.AddAsync(new OutboxMessage
            {
                Payload = JsonSerializer.Serialize(paymentSucceed),
                EventType = typeof(PaymentSucceedEvent).FullName,
            });
        }
        else
        {
            var paymentFailed = new PaymentFailedEvent
            {
                OrderNumber = response.OrderNumber,
            };

            await _outboxRepository.AddAsync(new OutboxMessage
            {
                Payload = JsonSerializer.Serialize(paymentFailed),
                EventType = typeof(PaymentFailedEvent).FullName,
            });
        }

        await _outboxRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return response;
    }
}