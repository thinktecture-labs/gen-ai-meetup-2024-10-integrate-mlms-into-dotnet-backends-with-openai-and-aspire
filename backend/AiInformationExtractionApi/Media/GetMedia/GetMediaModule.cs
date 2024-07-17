using Microsoft.Extensions.DependencyInjection;

namespace AiInformationExtractionApi.Media.GetMedia;

public static class GetMediaModule
{
    public static IServiceCollection AddGetMediaModule(this IServiceCollection services) =>
        services
           .AddScoped<IGetMediaDbSession, EfGetMediaSession>();
}