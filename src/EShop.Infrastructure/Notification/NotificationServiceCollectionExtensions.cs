using EShop.Infrastructure.Notification.Email;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.Notification;

public static class NotificationServiceCollectionExtensions
{
    public static IServiceCollection AddNotification(this IServiceCollection services, NotificationOptions options)
    {
        if (options.Email != null)
        {
            services.AddEmailNotification(options.Email);
        }

        return services;
    }
}
