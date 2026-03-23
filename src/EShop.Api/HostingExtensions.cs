using EShop.Api.Extensions;
using EShop.Api.Services;
using EShop.Application.Abstractions;
using EShop.Application.Catalog.Products.Services;
using EShop.Infrastructure.DateTimes;
using Microsoft.AspNetCore.HttpOverrides;
using EShop.ServiceDefaults.OpenApi;

namespace EShop.Api;

public static class HostingExtensions
{
    public static WebApplicationBuilder AddHosting(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        builder.Services.AddCors();

        //builder.Services.ConfigureApplicationCookie(options =>
        //{
        //    options.Cookie.SameSite = SameSiteMode.Lax;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //});

        //builder.Services.Configure<CookiePolicyOptions>(options =>
        //{
        //    options.MinimumSameSitePolicy = SameSiteMode.Lax;
        //    options.Secure = CookieSecurePolicy.SameAsRequest;
        //});

        builder.Services.AddJwt();

        //builder.AddCustomHealthCheck();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDefaultOpenApi();

        builder.Services.AddHttpContextAccessor();

        // Data Protection-keys: cookie auth, session, identity, antiforgery => persist key, encryptor
        //builder.Services.AddCustomDataProtection();

        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddScoped<ICurrentIPAddress, CurrentIPAddress>();

        builder.Services.AddScoped<IProductService, ProductService>();

        builder.Services.AddDateTimeProvider();
        //builder.Services.AddStorageManager();

        //builder.Services.AddHostedService<GracePeriodBackgroundService>();
        return builder;
    }

    public static WebApplication MapHosting(this WebApplication app)
    {
        app.UseForwardedHeaders();

        app.UseStaticFiles();

        app.UseCors(builder => builder
           .AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapDefaultEndpoints();

        if (app.Environment.IsEnvironment("Docker") || app.Environment.IsDevelopment())
        {
            app.UseDefaultOpenApi();
        }

        return app;
    }
}
