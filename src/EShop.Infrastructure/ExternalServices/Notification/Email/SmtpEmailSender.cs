using EShop.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using EShop.Shared.Constants;
using EShop.Shared.Web;
using System.Net;
using System.Net.Mail;

namespace EShop.Infrastructure.ExternalServices.Notifications.Email;

public class SmtpEmailSender : IEmailService
{
    private readonly IConfiguration _configuration;
    public SmtpEmailSender(IConfiguration configuration) => _configuration = configuration;

    public async Task SendAsync(string To, string Subject, string Message)
    {
        var mailConfig = _configuration.GetOptions<AppSettings>().MailConfig;

        var client = new SmtpClient(mailConfig.Host, mailConfig.Port);
        client.Credentials = new NetworkCredential(mailConfig.From, mailConfig.Password);
        client.EnableSsl = true;

        var message = new MailMessage(mailConfig.From, To, Subject, Message);

        await client.SendMailAsync(message);
    }
}
