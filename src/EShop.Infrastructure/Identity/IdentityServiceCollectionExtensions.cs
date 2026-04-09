using EShop.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.Identity;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentWebUser>();

        return services;
    }
}