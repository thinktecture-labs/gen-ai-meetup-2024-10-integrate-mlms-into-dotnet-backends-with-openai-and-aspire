using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

namespace Shared.CompositionRoot;

public static class ObservabilityExtensions
{
    public static void ConfigureDefaultLogging(this WebApplicationBuilder builder)
    {
        builder
           .Logging
           .ClearProviders()
           .AddOpenTelemetry(
                options =>
                {
                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;
                }
            );
        builder.Host.UseSerilog(
            (context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration),
            writeToProviders: true
        );
    }

    public static IServiceCollection AddOpenTelemetryMetricsAndTracing(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var openTelemetryBuilder = services
           .AddOpenTelemetry()
           .WithMetrics(
                metrics => metrics
                   .AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
                   .AddRuntimeInstrumentation()
            )
           .WithTracing(
                tracing => tracing
                   .AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
            );

        var useOpenTelemetryExporter = !configuration[Constants.OpenTelemetryExporterEndpoint].IsNullOrWhiteSpace();
        if (useOpenTelemetryExporter)
        {
            openTelemetryBuilder.UseOtlpExporter();
        }

        return services;
    }

    public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services)
    {
        services
           .AddHealthChecks()
           .AddCheck(Constants.Self, () => HealthCheckResult.Healthy(), [Constants.Live]);
        return services;
    }

    public static WebApplication MapDefaultHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks(Constants.HealthUrl);
        app.MapHealthChecks(
            Constants.AliveUrl,
            new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(Constants.Live)
            }
        );
        return app;
    }
}