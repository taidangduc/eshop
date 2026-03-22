namespace EShop.EventBus.RabbitMQ;

public class RabbitMqOptions
{
    public string ExchangeName { get; set; } = "eshop_event_bus";
    public string QueueName { get; set; } = "eshop_queue";
    public int MaxRetryCount { get; set; } = 3;
}
