using EShop.Application.Payments.Services;
using Stripe;
using Stripe.Checkout;

namespace EShop.Infrastructure.Payment.Stripe;

public class StripePaymentGateway : IPaymentGateway
{
    private readonly StripeOptions _options;
    public StripePaymentGateway(StripeOptions options)
    {
        _options = options;
        StripeConfiguration.ApiKey = _options.SecretKey;
    }

    public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
    {
        try
        {
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = request.Amount,
                            Currency = _options.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Order {request.OrderNumber}"
                            }
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{_options.ReturnUrl}?orderNumber={request.OrderNumber}",
                CancelUrl = $"{_options.ReturnUrl}?orderNumber={request.OrderNumber}&cancelled=true",
            };

            sessionOptions.Metadata = new Dictionary<string, string>
            {
                { "order_number", request.OrderNumber.ToString() }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(sessionOptions);

            return new CreatePaymentResponse()
            {
                IsSuccess = true,
                Message = session.Url
            };
        }
        catch (Exception ex)
        {
            return new CreatePaymentResponse()
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public Task<PaymentResultResponse> HandleReturnAsync(IDictionary<string, string> parameters)
    {
        return Task.FromResult(new PaymentResultResponse());
    }

    public async Task<PaymentResultResponse> HandleWebhookAsync(IDictionary<string, string> parameters)
    {
        if (!parameters.ContainsKey("payload") || !parameters.ContainsKey("sig_header"))
        {
            return new PaymentResultResponse
            {
                IsSuccess = false,
                Message = "Missing payload or signature header"
            };
        }

        var payload = parameters["payload"];
        var sigHeader = parameters["sig_header"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(payload, sigHeader, _options.WebhookSecret);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session == null)
                {
                    return new PaymentResultResponse { IsSuccess = false, Message = "Null event data" };
                }

                long orderNumber = 0;
                if (session.Metadata != null && session.Metadata.ContainsKey("order_number"))
                {
                    long.TryParse(session.Metadata["order_number"], out orderNumber);
                }

                decimal amount = Convert.ToDecimal(session.AmountTotal) / 100m;
                var transactionId = session.PaymentIntentId ?? string.Empty;

                return new PaymentResultResponse
                {
                    IsSuccess = session.PaymentStatus == "paid",
                    OrderNumber = orderNumber,
                    TransactionId = transactionId,
                    Amount = amount
                };
            }
            else if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var pi = stripeEvent.Data.Object as PaymentIntent;
                if (pi == null)
                {
                    return new PaymentResultResponse { IsSuccess = false, Message = "Null event data" };
                }

                long orderNumber = 0;
                if (pi.Metadata != null && pi.Metadata.ContainsKey("order_number"))
                {
                    long.TryParse(pi.Metadata["order_number"], out orderNumber);
                }

                decimal amount = Convert.ToDecimal(pi.Amount) / 100m;

                return new PaymentResultResponse
                {
                    IsSuccess = true,
                    Message = "Payment succeeded",
                    OrderNumber = orderNumber,
                    TransactionId = pi.Id,
                    Amount = amount,
                };
            }
            return new PaymentResultResponse { IsSuccess = false, Message = "Unhandled event type" };
        }
        catch (StripeException)
        {
            return new PaymentResultResponse { IsSuccess = false, Message = "Stripe exception occurred" };
        }
        catch (Exception)
        {
            return new PaymentResultResponse { IsSuccess = false, Message = "General exception occurred" };
        }
    }
}

