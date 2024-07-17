using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.Analyze.Prompting;
using AiInformationExtractionApi.OpenAiAccess;
using Light.SharedCore.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared;
using Shared.Messages.Analyze;

namespace AiInformationExtractionApi.Analyze.AnalyzeText;

public static class AnalyzeTextEndpoint
{
    public static void MapAnalyzeTextEndpoint(this WebApplication app) =>
        app.MapPut("/api/analyze/text", AnalyzeText)
           .WithName("AnalyzeText")
           .WithTags(Constants.Analyze)
           .Produces<AnalysisResponseDto>()
           .Produces(StatusCodes.Status500InternalServerError)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> AnalyzeText(
        AnalyzeTextRequestDto dto,
        IAiChatClient aiChatClient,
        PromptManager promptManager,
        IClock clock,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        var additionalInformation = dto.ExistingDamageReportData.CreateAdditionalPromptInformation();
        var messages = promptManager.CreateTextAnalysisPrompt(
            dto.FormSection,
            dto.TextToAnalyze,
            additionalInformation
        );
        return await aiChatClient.CompleteAnalysisAsync(
            AnalysisType.Text,
            dto.FormSection,
            messages,
            clock,
            logger,
            cancellationToken
        );
    }
}