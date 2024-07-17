using Microsoft.Extensions.DependencyInjection;

namespace AiInformationExtractionApi.Analyze.AnalyzeImage;

public static class AnalyzeImageModule
{
    public static IServiceCollection AddAnalyzeImageModule(this IServiceCollection services) =>
        services.AddScoped<IAnalyzeImageDbSession, EfAnalyzeImageSession>();
}