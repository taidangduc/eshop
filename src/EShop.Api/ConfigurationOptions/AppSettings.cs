using EShop.EventBus.RabbitMQ;
using EShop.Infrastructure.Notification;
using EShop.Infrastructure.Payment;
using EShop.Infrastructure.Storage;

namespace EShop.Api.ConfigurationOptions;

public class AppSettings
{
    public StorageOptions Storage { get; set; }
    public CORS CORS { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public NotificationOptions Notification { get; set; }
    public PaymentOptions Payment { get; set; }
    public RabbitMQOptions RabbitMQ { get; set; }
}