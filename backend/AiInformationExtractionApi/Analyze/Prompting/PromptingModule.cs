using Microsoft.Extensions.DependencyInjection;

namespace AiInformationExtractionApi.Analyze.Prompting;

public static class PromptingModule
{
    public static IServiceCollection AddPromptingModule(this IServiceCollection services) =>
        services.AddSingleton<PromptManager>();
}