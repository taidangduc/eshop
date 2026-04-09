namespace EShop.EventBus.RabbitMQ;

public class RabbitMQReceiverOptions
{
    public string HostName { get; set; }
    public int Port { get; set; } = 5672;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
    public bool AutomaticCreateEnabled { get; set; }
    public string QueueType { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string RoutingKey { get; set; }
}