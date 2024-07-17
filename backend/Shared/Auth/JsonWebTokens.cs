using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Auth;

public static class JsonWebTokens
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
           .AddAuthentication("Bearer")
           .ConfigureJwtBearer(configuration);

        services.AddAuthorization();

        return services;
    }

    public static AuthenticationBuilder ConfigureJwtBearer(
        this AuthenticationBuilder builder,
        IConfiguration configuration
    ) =>
        builder
           .AddJwtBearer(
                "Bearer",
                options =>
                {
                    var identityServerUri = configuration["services:IdentityServer:https:0"];
                    options.Authority = identityServerUri;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = identityServerUri,
                        ValidateLifetime = true,
                        ValidateAudience = false
                    };
                }
            );
}