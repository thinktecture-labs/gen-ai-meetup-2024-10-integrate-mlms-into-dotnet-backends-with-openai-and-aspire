using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.OpenAiAccess;
using Light.GuardClauses;
using Light.SharedCore.Time;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OpenAI.Chat;
using Serilog;
using Shared.JsonAccess;
using Shared.Messages.Analyze;

namespace AiInformationExtractionApi.Analyze;

public static class TextCompletionExtensions
{
    public static async Task<Ok<AnalysisResponseDto>> CompleteAnalysisAsync(
        this IAiChatClient aiChatClient,
        AnalysisType analysisType,
        FormSection formSection,
        List<ChatMessage> messages,
        IClock clock,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        var now = clock.GetTime();
        var response = await aiChatClient.CompleteChatAsync(messages, cancellationToken);
        response.Content.MustHaveCount(1);
        var content = response.Content[0];
        content.Kind.MustBe(ChatMessageContentPartKind.Text);
        logger.Debug("Received response from Open AI Image analysis\n{Response}", content.Text);
        using var jsonDocument = Json.ParseDocument(content.Text);
        return TypedResults.Ok(
            new AnalysisResponseDto(
                analysisType,
                formSection,
                now,
                // We clone the root element to avoid issues when the jsonDocument is disposed
                jsonDocument.RootElement.Clone()
            )
        );
    }
}