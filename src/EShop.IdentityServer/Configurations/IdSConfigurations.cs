using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using EShop.IdentityService.Authorization;

namespace EShop.IdentityService.Configurations;

public static class IdSConfigurations
{
    // Resource - ApiScope - ApiResource - Clients
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    ];

    public static IEnumerable<ApiScope> ApiScopes => [
        new ApiScope(Permissions.Read, "Read Access to API"),
        new ApiScope(Permissions.Write, "Write Access to API"),
        new ApiScope(Permissions.All, "Write and Read Access to API")
    ];

    public static IEnumerable<ApiResource> ApiResources => [
        new()
        {
            Name = "api.eshop",
            DisplayName = "Eshop API",
            Scopes = {Permissions.Read, Permissions.Write},
        }
    ];

    public static IEnumerable<Client> Clients => [

        // Resource Owner
        new()
        {
            ClientId = "ro.client",
            ClientName = "Resource Owner Client",

            ClientSecrets = {new Secret("ro.client.secret".Sha256())},
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

            AllowedScopes = { Permissions.Read, Permissions.Write},
        },
        // Backend For Frontend
        new()
        {
            ClientId = "bff",
            ClientName = "Backend For Frontend",

            ClientSecrets = {new Secret("bff.secret".Sha256())},
            AllowedGrantTypes = GrantTypes.Code,

            AllowedCorsOrigins = { "https://localhost:5002"},
            RedirectUris = { "https://localhost:5002/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc"},
            FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",

            AllowOfflineAccess = true,
            AlwaysIncludeUserClaimsInIdToken = true,
            AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, Permissions.Read, Permissions.Write},
        },
    ];
}
