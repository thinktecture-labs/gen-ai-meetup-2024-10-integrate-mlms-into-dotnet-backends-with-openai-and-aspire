using FluentValidation;

namespace AiInformationExtractionApi.OpenAiAccess;

public sealed class OpenAiOptionsValidator : AbstractValidator<OpenAiOptions>
{
    public OpenAiOptionsValidator()
    {
        RuleFor(options => options.ApiKey).NotEmpty();
        RuleFor(options => options.GptModel).NotEmpty();
        RuleFor(options => options.AudioTranscriptionModel).NotEmpty();
    }
}