using EShop.Application.Payment.Dtos;
using EShop.Domain.Enums;
using MediatR;

namespace EShop.Application.Payment.Commands.VerifyPaymentReturn;

public record VerifyPaymentReturnCommand(
    PaymentProvider Provider, 
    IDictionary<string, string> Parameters) 
    : IRequest<PaymentReturnResult>;
