using EShop.Infrastructure.Payment.Stripe;

namespace EShop.Infrastructure.Payment;

public class PaymentOptions
{
    public StripeOptions Stripe { get; set; }    
}
