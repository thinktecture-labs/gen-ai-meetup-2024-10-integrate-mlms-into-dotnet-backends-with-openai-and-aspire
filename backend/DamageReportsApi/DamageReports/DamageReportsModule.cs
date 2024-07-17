using DamageReportsApi.DamageReports.GetDamageReport;
using DamageReportsApi.DamageReports.GetDamageReports;
using DamageReportsApi.DamageReports.SubmitDamageReport;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DamageReportsApi.DamageReports;

public static class DamageReportsModule
{
    public static IServiceCollection AddDamageReports(this IServiceCollection services) =>
        services
           .AddGetDamageReportsModule()
           .AddGetDamageReportModule()
           .AddSubmitDamageReportModule();

    public static void MapDamageReportsEndpoints(this WebApplication app)
    {
        app.MapGetDamageReports();
        app.MapGetDamageReport();
        app.MapSubmitDamageReport();
    }
}