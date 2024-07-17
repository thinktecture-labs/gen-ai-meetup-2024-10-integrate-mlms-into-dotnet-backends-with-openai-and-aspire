using AiInformationExtractionApi.Analyze.AnalyzeImage;
using AiInformationExtractionApi.Analyze.AnalyzeSpeech;
using AiInformationExtractionApi.Analyze.AnalyzeText;
using AiInformationExtractionApi.Analyze.Prompting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AiInformationExtractionApi.Analyze;

public static class AnalyzeModule
{
    public static IServiceCollection AddAnalyzeModule(this IServiceCollection services) =>
        services
           .AddAnalyzeImageModule()
           .AddPromptingModule();

    public static void MapAnalyzeEndpoints(this WebApplication app)
    {
        app.MapAnalyzeTextEndpoint();
        app.MapAnalyzeSpeechEndpoint();
        app.MapAnalyzeImageEndpoint();
    }
}