using AiInformationExtractionApi.Analyze;
using AiInformationExtractionApi.Media;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Shared.CompositionRoot;

namespace AiInformationExtractionApi.CompositionRoot;

public static class MiddlewareConfiguration
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseOpenApi();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapDefaultHealthChecks();
        app.MapAnalyzeEndpoints();
        app.MapMediaEndpoints();
        return app;
    }
}