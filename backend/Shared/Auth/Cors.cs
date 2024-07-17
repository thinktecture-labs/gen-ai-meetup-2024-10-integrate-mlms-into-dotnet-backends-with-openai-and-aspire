using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Auth;

public static class Cors
{
    public static IServiceCollection AddCorsSupport(this IServiceCollection services) =>
        services
           .AddSingleton(
                sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var corsOptions = new CorsOptions();
                    configuration.GetSection(Constants.Cors).Bind(corsOptions);
                    return corsOptions;
                }
            )
           .AddCors();

    public static void UseCorsIfNecessary(this WebApplication app)
    {
        var corsOptions = app.Services.GetRequiredService<CorsOptions>();
        if (corsOptions.IsCorsEnabled)
        {
            app.UseCors(
                policyBuilder => policyBuilder
                   .WithOrigins(corsOptions.AllowedOrigins)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
            );
        }
    }
}

public sealed class CorsOptions
{
    public string[] AllowedOrigins { get; init; } = [];

    public bool IsCorsEnabled => !AllowedOrigins.IsNullOrEmpty();
}