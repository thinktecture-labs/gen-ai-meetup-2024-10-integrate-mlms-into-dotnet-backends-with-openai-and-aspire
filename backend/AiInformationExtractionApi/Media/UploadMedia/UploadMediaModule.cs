using Microsoft.Extensions.DependencyInjection;

namespace AiInformationExtractionApi.Media.UploadMedia;

public static class UploadMediaModule
{
    public static IServiceCollection AddUploadMediaModule(this IServiceCollection services) =>
        services.AddScoped<IUploadMediaDbSession, EfUploadMediaSession>();
}