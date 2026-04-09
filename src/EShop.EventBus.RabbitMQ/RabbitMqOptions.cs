namespace EShop.EventBus.RabbitMQ;

public class RabbitMQOptions
{
    public string HostName { get; set; }
    public int Port { get; set; } = 5672;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public Dictionary<string, string> RoutingKeys { get; set; }
    public Dictionary<string, Dictionary<string, string>> Consumers { get; set; }
    public string ConnectionString
    {
        get
        {
            return $"amqp://{UserName}:{Password}@{HostName}:{Port}";
        }
    }
}