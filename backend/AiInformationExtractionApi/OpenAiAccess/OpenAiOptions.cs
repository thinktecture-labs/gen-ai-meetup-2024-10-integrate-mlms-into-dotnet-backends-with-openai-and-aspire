using System;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using Microsoft.Extensions.Configuration;

namespace AiInformationExtractionApi.OpenAiAccess;

public sealed record OpenAiOptions
{
    public string ApiKey { get; init; } = string.Empty;
    public string GptModel { get; init; } = "gpt-4o";
    public string AudioTranscriptionModel { get; init; } = "whisper-1";

    public static OpenAiOptions FromConfiguration(IConfiguration configuration, string sectionName = "openAi")
    {
        sectionName.MustNotBeNullOrWhiteSpace();
        var options = configuration.GetSection(sectionName).Get<OpenAiOptions>() ?? new OpenAiOptions();
        var validationResults = new OpenAiOptionsValidator().Validate(options);
        if (validationResults.IsValid)
        {
            return options;
        }

        var errorMessage = $"OpenAI options are invalid{Environment.NewLine}{validationResults}";
        throw new InvalidConfigurationException(errorMessage);
    }
}