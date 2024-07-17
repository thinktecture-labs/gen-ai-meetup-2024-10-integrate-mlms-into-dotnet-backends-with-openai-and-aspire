using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.Analyze.Prompting;
using AiInformationExtractionApi.OpenAiAccess;
using Light.GuardClauses;
using Light.SharedCore.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Serilog;
using Shared;
using Shared.Messages.Analyze;

namespace AiInformationExtractionApi.Analyze.AnalyzeSpeech;

public static class AnalyzeSpeechEndpoint
{
    public static void MapAnalyzeSpeechEndpoint(this WebApplication app) =>
        app.MapPut("/api/analyze/speech", AnalyzeSpeech)
           .WithName("AnalyzeSpeech")
           .WithTags(Constants.Analyze)
           .Produces<AnalysisResponseDto>()
           .Produces<string>(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> AnalyzeSpeech(
        HttpRequest httpRequest,
        IAiAudioClient audioClient,
        IAiChatClient chatClient,
        PromptManager promptManager,
        IOptions<JsonOptions> jsonOptions,
        IClock clock,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        ProcessingResults processingResults;
        try
        {
            processingResults = await ProcessMultipartRequest(
                httpRequest,
                jsonOptions.Value.SerializerOptions,
                cancellationToken
            );
        }
        catch (InvalidDataException e)
        {
            logger.Warning(e, "Could not parse the multipart request");
            return TypedResults.BadRequest(e.Message);
        }

        var text = await audioClient.TranscribeAudioAsync(processingResults.AudioFilePath);

        var additionalInformation = processingResults.Dto.ExistingDamageReportData.CreateAdditionalPromptInformation();
        var messages = promptManager.CreateTextAnalysisPrompt(
            processingResults.Dto.FormSection,
            text,
            additionalInformation
        );
        var result = await chatClient.CompleteAnalysisAsync(
            AnalysisType.Speech,
            processingResults.Dto.FormSection,
            messages,
            clock,
            logger,
            cancellationToken
        );
        return result;
    }

    private static async Task<ProcessingResults> ProcessMultipartRequest(
        HttpRequest request,
        JsonSerializerOptions jsonOptions,
        CancellationToken cancellationToken
    )
    {
        if (!MediaTypeHeaderValue.TryParse(request.ContentType, out var contentType) ||
            !contentType.MediaType.Equals("multipart/form-data", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("The request' content type is not multipart/form-data");
        }

        var boundary = GetBoundary(contentType, 70);
        var multipartReader = new MultipartReader(boundary, request.Body);

        AnalyzeSpeechRequestDto? jsonData = default;
        string? filePath = default;

        while (await multipartReader.ReadNextSectionAsync(cancellationToken) is { } section)
        {
            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var parsedContentDisposition))
            {
                throw new InvalidDataException(
                    "Not all multipart/form-data sections have a content disposition header"
                );
            }

            if (!MediaTypeHeaderValue.TryParse(section.ContentType, out var sectionType))
            {
                throw new InvalidDataException("One of the sections does not have a content type");
            }

            if (sectionType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                if (jsonData is not null)
                {
                    throw new InvalidDataException("The request has several application/json sections");
                }

                jsonData = JsonSerializer.Deserialize<AnalyzeSpeechRequestDto>(section.Body, jsonOptions);
            }
            else
            {
                if (filePath is not null)
                {
                    throw new InvalidDataException("The request has several binary sections ");
                }

                var extension = Path.GetExtension(parsedContentDisposition.FileName.Value);
                if (extension.IsNullOrWhiteSpace())
                {
                    throw new InvalidOperationException("The audio file name does not have an extension");
                }

                Directory.CreateDirectory("./audio");
                filePath = $"./audio/{Guid.NewGuid()}{extension}";
                await using var fileStream = File.Create(filePath);
                await section.Body.CopyToAsync(fileStream, cancellationToken);
            }
        }

        if (jsonData is null || filePath is null)
        {
            throw new InvalidDataException("The request does not contain both JSON and binary audio data");
        }

        return new ProcessingResults(jsonData, filePath);
    }

    private static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
        if (StringSegment.IsNullOrEmpty(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        if (boundary.Length > lengthLimit)
        {
            throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
        }

        return boundary.ToString();
    }

    private readonly record struct ProcessingResults(
        AnalyzeSpeechRequestDto Dto,
        string AudioFilePath
    );
}