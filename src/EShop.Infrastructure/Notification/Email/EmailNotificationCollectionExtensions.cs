using EShop.Domain.Notification;
using EShop.Infrastructure.Notification.Email.SmtpClient;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.Notification.Email;

public static class EmailNotificationCollectionExtensions
{
    public static IServiceCollection AddEmailNotification(this IServiceCollection services, EmailOptions options)
    {
        services.AddSingleton<IEmailNotification>(new SmtpClientEmailNotification(options.SmtpClient));

        return services;
    }
}