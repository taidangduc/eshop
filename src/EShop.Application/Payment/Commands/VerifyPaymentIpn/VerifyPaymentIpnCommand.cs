using EShop.Application.Payment.Dtos;
using EShop.Domain.Enums;
using MediatR;

namespace EShop.Application.Payment.Commands.VerifyPaymentIpn;

public record VerifyPaymentIpnCommand(
    PaymentProvider Provider,
    IDictionary<string, string> Parameters) 
    : IRequest<IpnResult>;
