using Microsoft.Extensions.DependencyInjection;

namespace DamageReportsApi.DamageReports.GetDamageReport;

public static class GetDamageReportModule
{
    public static IServiceCollection AddGetDamageReportModule(this IServiceCollection services) =>
        services.AddScoped<IGetDamageReportDbSession, EfGetDamageReportSession>();
}