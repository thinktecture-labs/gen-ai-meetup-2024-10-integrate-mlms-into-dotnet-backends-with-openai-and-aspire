using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.JsonAccess;

public static class JsonConfiguration
{
    public static IServiceCollection AddDefaultHttpJsonOptions(
        this IServiceCollection services,
        bool includeMvcOptions = true
    )
    {
        services.ConfigureHttpJsonOptions(
            options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

        // This is required for Swashbuckle to use string values for enums in the generated Swagger UI
        if (includeMvcOptions)
        {
            services.Configure<JsonOptions>(
                options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );
        }

        return services;
    }
}