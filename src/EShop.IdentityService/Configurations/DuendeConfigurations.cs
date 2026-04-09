using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using EShop.IdentityService.Constants;

namespace EShop.IdentityService.Configurations;

public static class DuendeConfigurations
{
    // Resource - ApiScope - ApiResource - Clients
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    ];

    public static IEnumerable<ApiScope> ApiScopes => [
        new ApiScope(AuthorizationScope.Read, "Read Access to API"),
        new ApiScope(AuthorizationScope.Write, "Write Access to API"),
        new ApiScope(AuthorizationScope.All, "Write and Read Access to API")
    ];

    public static IEnumerable<ApiResource> ApiResources => [
        new()
        {
            Name = "api.eshop",
            DisplayName = "Eshop API",
            Scopes = {AuthorizationScope.Read, AuthorizationScope.Write},
        }
    ];

    public static IEnumerable<Client> Clients => [

        // Resource Owner
        new()
        {
            ClientId = "ro.client",
            ClientName = "Resource Owner Client",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes = { AuthorizationScope.Read, AuthorizationScope.Write},
        },
        // Backend For Frontend
        new()
        {
            ClientId = "bff",
            ClientName = "Backend For Frontend",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = [GrantType.AuthorizationCode],
            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                AuthorizationScope.Read,
                AuthorizationScope.Write,
            },
            AllowOfflineAccess = true,
            AllowedCorsOrigins = { "https://localhost:5002"},
            AlwaysIncludeUserClaimsInIdToken = true,          
            RedirectUris = { "https://localhost:5002/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc"},
            FrontChannelLogoutUri = "https://localhost:5002/signout-oidc"
        },     
    ];
}
