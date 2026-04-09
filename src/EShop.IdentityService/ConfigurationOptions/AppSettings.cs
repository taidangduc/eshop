using EShop.EventBus.RabbitMQ;
using EShop.IdentityService.ConfigurationOptions.ExternalLogin;

namespace EShop.IdentityService.ConfigurationOptions;

public class AppSettings
{
    public ExternalLoginOptions ExternalLogin { get; set; }
    public RabbitMQOptions RabbitMQ { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}
