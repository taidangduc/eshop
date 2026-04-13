using EShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Api.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services)
    {
        //var jwtOptions = services.GetOptions<Identity>("Identity");
        //.AddJwtBearer(options =>
        //{
        //    options.Authority = jwtOptions.Authority;
        //    options.Audience = jwtOptions.Audience;
        //    options.RequireHttpsMetadata = false; // required https

        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidIssuer = jwtOptions.Authority,
        //        ValidateAudience = true,
        //        ValidAudience = jwtOptions.Audience,
        //        ValidateLifetime = true,
        //        ClockSkew = TimeSpan.FromSeconds(2), // Reduce default clock skew

        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),

        //        NameClaimType = ClaimTypes.Name,
        //        RoleClaimType = ClaimTypes.Role,
        //    };

        //    options.MapInboundClaims = false;
        //});


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
           {
               options.Audience = "eshop";
               options.Authority = "https://localhost:5001";
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters.ValidateAudience = false;
           });

        services.AddAuthorization(
            options =>
            {
                //Role-bases
                options.AddPolicy(
                    Authorization.Roles.Admin,
                    x =>
                    {
                        x.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        x.RequireRole(Authorization.Roles.Admin);
                    }
                );
                options.AddPolicy(
                    Authorization.Roles.User,
                    x =>
                    {
                        x.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        x.RequireRole(Authorization.Roles.User);
                    }
                );
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                policy.RequireAuthenticatedUser())
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }
}
