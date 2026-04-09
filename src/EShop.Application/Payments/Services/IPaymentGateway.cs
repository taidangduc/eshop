namespace EShop.Application.Payments.Services;

public interface IPaymentGateway
{
    Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
    Task<PaymentResultResponse> HandleReturnAsync(IDictionary<string, string> parameters);
    Task<PaymentResultResponse> HandleWebhookAsync(IDictionary<string, string> parameters);
}

public class CreatePaymentRequest
{
    public long OrderNumber { get; set; }
    public decimal Amount { get; set; }
}

public class CreatePaymentResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}

public class PaymentResultResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public string? TransactionId { get; set; }
    public long OrderNumber { get; set; }
    public decimal Amount { get; set; }
    public string? CardBrand { get; set; }
    public Dictionary<string, string>? MetaData { get; set; }
}