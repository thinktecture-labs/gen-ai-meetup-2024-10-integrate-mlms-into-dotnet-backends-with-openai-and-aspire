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

namespace AiInformationExtractionApi.Analyze.AnalyzeImage;

public static class AnalyzeImageEndpoint
{
    public static void MapAnalyzeImageEndpoint(this WebApplication app) =>
        app.MapPut("/api/analyze/image", AnalyzeImage)
           .WithName("AnalyzeImage")
           .WithTags(Constants.Analyze)
           .Produces<AnalysisResponseDto>()
           .Produces(StatusCodes.Status500InternalServerError)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> AnalyzeImage(
        AnalyzeImageRequestDto dto,
        IAnalyzeImageDbSession dbSession,
        PromptManager promptManager,
        IAiChatClient aiChatClient,
        IClock clock,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        var mediaItem = await dbSession.GetMediaItemAsync(dto.ImageId, cancellationToken);
        if (mediaItem == null)
        {
            return TypedResults.NotFound();
        }

        var additionalInformation = dto.ExistingDamageReportData.CreateAdditionalPromptInformation();
        var messages = promptManager.CreateImageAnalysisPrompt(
            dto.FormSection,
            mediaItem.Content,
            mediaItem.MimeType,
            additionalInformation
        );
        return await aiChatClient.CompleteAnalysisAsync(
            AnalysisType.Image,
            dto.FormSection,
            messages,
            clock,
            logger,
            cancellationToken
        );
    }
}