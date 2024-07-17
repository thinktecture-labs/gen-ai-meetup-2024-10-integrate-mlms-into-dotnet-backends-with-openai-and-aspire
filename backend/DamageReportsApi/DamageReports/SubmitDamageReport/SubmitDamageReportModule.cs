using Microsoft.Extensions.DependencyInjection;

namespace DamageReportsApi.DamageReports.SubmitDamageReport;

public static class SubmitDamageReportModule
{
    public static IServiceCollection AddSubmitDamageReportModule(this IServiceCollection services) =>
        services.AddScoped<ISubmitDamageReportDbSession, EfSubmitDamageReportSession>();
}