using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.CompositionRoot;

public static class OpenApi
{
    public static IServiceCollection AddOpenApiSupport(this IServiceCollection services) =>
        services
           .AddEndpointsApiExplorer()
           .AddSwaggerGen();

    public static void UseOpenApi(this WebApplication app) =>
        app.UseSwagger()
           .UseSwaggerUI();
}