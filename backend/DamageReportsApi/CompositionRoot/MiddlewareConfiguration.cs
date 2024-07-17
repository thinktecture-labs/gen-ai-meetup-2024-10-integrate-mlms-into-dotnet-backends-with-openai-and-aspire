using DamageReportsApi.DamageReports;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Shared.CompositionRoot;

namespace DamageReportsApi.CompositionRoot;

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
        app.MapDamageReportsEndpoints();
        app.MapRedirectToSwagger();
        return app;
    }
}