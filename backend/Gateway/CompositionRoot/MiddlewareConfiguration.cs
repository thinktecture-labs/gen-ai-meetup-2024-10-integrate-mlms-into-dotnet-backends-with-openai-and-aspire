using Microsoft.AspNetCore.Builder;
using Shared.CompositionRoot;

namespace Gateway.CompositionRoot;

public static class MiddlewareConfiguration
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapDefaultHealthChecks();
        app.MapReverseProxy().RequireAuthorization();
        return app;
    }
}