using Microsoft.Extensions.DependencyInjection;

namespace DamageReportsApi.DamageReports.GetDamageReports;

public static class GetDamageReportsModule
{
    public static IServiceCollection AddGetDamageReportsModule(this IServiceCollection services) =>
        services.AddScoped<IGetDamageReportsDbSession, EfGetDamageReportsSession>();
}