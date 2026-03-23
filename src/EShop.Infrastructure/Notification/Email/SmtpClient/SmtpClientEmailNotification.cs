using EShop.Domain.Notification;
using System.Net.Mail;
using System.Net;

namespace EShop.Infrastructure.Notification.Email.SmtpClient;

public class SmtpClientEmailNotification : IEmailNotification
{
    private readonly SmtpClientOptions _options;

    public SmtpClientEmailNotification(SmtpClientOptions options)
    {
        _options = options;
    }

    public async Task SendAsync(IEmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var mail = new MailMessage();

        mail.From = new MailAddress(emailMessage.From);
        mail.To.Add(emailMessage.To);
        mail.Subject = emailMessage.Subject;
        mail.Body = emailMessage.Body;
        mail.IsBodyHtml = true;

        var smtpClient = new System.Net.Mail.SmtpClient(_options.Host, _options.Port)
        {
            Credentials = new NetworkCredential(_options.UserName, _options.Password),
            EnableSsl = _options.EnableSsl
        };

        await smtpClient.SendMailAsync(mail, cancellationToken);
    }
}