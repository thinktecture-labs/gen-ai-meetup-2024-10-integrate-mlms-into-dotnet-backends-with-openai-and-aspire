using AiInformationExtractionApi.Media.GetMedia;
using AiInformationExtractionApi.Media.UploadMedia;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AiInformationExtractionApi.Media;

public static class MediaModule
{
    public static IServiceCollection AddMediaModule(this IServiceCollection services) =>
        services
           .AddGetMediaModule()
           .AddUploadMediaModule();

    public static void MapMediaEndpoints(this WebApplication app)
    {
        app.MapGetMediaEndpoint();
        app.MapUploadMediaEndpoint();
    }
}