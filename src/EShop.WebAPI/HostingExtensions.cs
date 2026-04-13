using EShop.Api.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using EShop.ServiceDefaults.OpenApi;
using EShop.Api.ConfigurationOptions;
using EShop.Infrastructure.Identity;
using EShop.Infrastructure.HostServices;

namespace EShop.Api;

public static class HostingExtensions
{
    public static IServiceCollection AddHosting(this IServiceCollection services, AppSettings appSettings)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins", builder =>
            {
                builder.WithOrigins(appSettings.CORS.AllowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        services.AddJwt();

        //services.AddCustomHealthCheck();

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddDefaultOpenApi();

        services.AddHttpContextAccessor();

        // Data Protection-keys: cookie auth, session, identity, antiforgery => persist key, encryptor
        //services.AddCustomDataProtection();

        services.AddIdentity();
        services.AddHostServices();

        return services;
    }

    public static WebApplication MapHosting(this WebApplication app)
    {
        app.UseForwardedHeaders();

        app.UseStaticFiles();

        app.UseCors("AllowedOrigins");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapDefaultEndpoints();

        app.MapControllers();

        if (app.Environment.IsEnvironment("Docker") || app.Environment.IsDevelopment())
        {
            app.UseDefaultOpenApi();
        }

        return app;
    }

    static void AddHostServices(this IServiceCollection services)
    {
        //services.AddHostedService<GracePeriodWorker>();
    }
}
