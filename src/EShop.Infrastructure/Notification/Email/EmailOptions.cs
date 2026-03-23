using EShop.Infrastructure.Notification.Email.SmtpClient;

namespace EShop.Infrastructure.Notification.Email;

public class EmailOptions
{
    public SmtpClientOptions SmtpClient { get; set; }
}