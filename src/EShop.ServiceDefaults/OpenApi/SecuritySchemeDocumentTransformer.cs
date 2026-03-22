using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace EShop.ServiceDefaults.OpenApi;

public class SecuritySchemeDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new();

        // Bearer token scheme  
        document.Components.SecuritySchemes.Add(
            "Bearer",
           new OpenApiSecurityScheme
           {
               Name = "Authorization",
               Type = SecuritySchemeType.Http,
               BearerFormat = "JWT",
               Scheme = "bearer",
               In = ParameterLocation.Header,
               Description =
                        "Enter 'Bearer' [space] and your token in the text input below.\n\nExample: 'Bearer mlJhbXoiOiJIUzI1NiIsInR5cCI6IkpXKvJ9...'",
           });

        // Require
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // Api Key scheme
#if (ApiKey)
        document.Components.SecuritySchemes.Add(
            "ApiKey",
            new OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description =
                    "Enter your API key in the text input below.\n\nExample: '13123-aSFdef'",
            });
#endif

        return Task.CompletedTask;
    }
}
