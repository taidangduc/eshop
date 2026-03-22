using EShop.Application.Abstractions;
using EShop.Domain.Enums;
using MediatR;

namespace EShop.Application.Payment.Commands.CreatePaymentUrl;

public record CreatePaymentUrlCommand(
    long OrderNumber, 
    decimal Amount,
    PaymentProvider Provider, 
    DateTime OrderDate) 
    : IRequest<CreatePaymentUrlResult>;
