using Microsoft.Extensions.DependencyInjection;

namespace Shared.CompositionRoot;

public static class Http
{
    public static IServiceCollection AddHttpClientDefaults(this IServiceCollection services) =>
        services.ConfigureHttpClientDefaults(
            builder =>
            {
                builder.AddStandardResilienceHandler();
                builder.AddServiceDiscovery();
            }
        );
}