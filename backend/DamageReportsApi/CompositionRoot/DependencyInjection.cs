using DamageReportsApi.DamageReports;
using DamageReportsApi.DatabaseAccess;
using Light.SharedCore.Time;
using Microsoft.AspNetCore.Builder;
using Shared.Auth;
using Shared.CompositionRoot;
using Shared.JsonAccess;
using Shared.Messages.DamageReports;
using Shared.Paging;

namespace DamageReportsApi.CompositionRoot;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureDefaultLogging();
        builder.AddDatabaseAccess();

        builder
           .Services
           .AddUtcClock()
           .AddDefaultHttpJsonOptions()
           .AddOpenTelemetryMetricsAndTracing(builder.Configuration)
           .AddJwtAuth(builder.Configuration)
           .AddHttpClientDefaults()
           .AddDefaultHealthChecks()
           .AddOpenApiSupport()
           .AddPagingSupport()
           .AddMessageValidators()
           .AddDamageReports();
        return builder;
    }
}