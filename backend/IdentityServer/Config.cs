using Duende.IdentityServer.Models;

namespace IdentityServer;

public sealed class Config
{
    private readonly IConfiguration _configuration;

    public Config(IConfiguration configuration) => _configuration = configuration;

    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new ("webapi", "Web API")
    ];

    public IEnumerable<Client> Clients
    {
        get
        {
            var redirectUri = _configuration["services:Gateway:https:0"];
            yield return new Client
            {
                ClientId = "spa",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { $"{redirectUri}/signin-oidc" },
                FrontChannelLogoutUri = $"{redirectUri}/signout-oidc",
                PostLogoutRedirectUris = { $"{redirectUri}/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "webapi" }
            };
            yield return new Client
            {
                ClientId = "integration-tests",
                ClientSecrets = { new Secret("password".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "webapi" }
            };
        }
    }
}