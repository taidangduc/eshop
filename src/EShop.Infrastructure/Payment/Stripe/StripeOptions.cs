namespace EShop.Infrastructure.Payment.Stripe;

public class StripeOptions
{
    public string SecretKey { get; set; }
    public string WebhookSecret { get; set; }
    public string ReturnUrl { get; set; }
    public string Currency { get; set; }
}
