using AiInformationExtractionApi.Analyze;
using AiInformationExtractionApi.DatabaseAccess;
using AiInformationExtractionApi.Media;
using AiInformationExtractionApi.OpenAiAccess;
using Light.SharedCore.Time;
using Microsoft.AspNetCore.Builder;
using Shared.Auth;
using Shared.CompositionRoot;
using Shared.JsonAccess;

namespace AiInformationExtractionApi.CompositionRoot;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureDefaultLogging();
        builder.AddDatabaseAccess();

        builder
           .Services
           .AddUtcClock()
           .AddDefaultHealthChecks()
           .AddDefaultHttpJsonOptions()
           .AddOpenTelemetryMetricsAndTracing(builder.Configuration)
           .AddJwtAuth(builder.Configuration)
           .AddOpenAiAccess()
           .AddOpenApiSupport()
           .AddOpenAiAccess()
           .AddAnalyzeModule()
           .AddMediaModule();
        
        return builder;
    }
}