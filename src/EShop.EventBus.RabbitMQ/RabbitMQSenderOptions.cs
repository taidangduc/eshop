namespace EShop.EventBus.RabbitMQ;

public class RabbitMQSenderOptions
{
    public string HostName { get; set; }
    public int Port { get; set; } = 5672;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string RoutingKey { get; set; }
}