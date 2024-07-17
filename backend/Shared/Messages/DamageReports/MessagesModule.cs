using Microsoft.Extensions.DependencyInjection;

namespace Shared.Messages.DamageReports;

public static class MessagesModule
{
    public static IServiceCollection AddMessageValidators(this IServiceCollection services) =>
        services
           .AddSingleton<SubmitDamageReportDtoValidator>()
           .AddSingleton<PersonalDataDtoValidator>()
           .AddSingleton<CircumstancesDtoValidator>()
           .AddSingleton<VehicleDamageDtoValidator>()
           .AddSingleton<PersonDtoValidator>();
}