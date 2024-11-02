using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Light.EmbeddedResources;
using Shared;
using Shared.Messages.Analyze;
using Shared.Messages.DamageReports;
using Xunit;

namespace AspireAppHost.IntegrationTests;

[Collection(nameof(AspireCollection))]
public sealed class ImageIntegrationTests
{
    private readonly AspireFixture _fixture;

    public ImageIntegrationTests(AspireFixture fixture) => _fixture = fixture;

    [SkippableFact]
    public async Task AnalyzeImage()
    {
        AspireFixture.SkipTestIfNecessary();

        using var httpClient =
            await _fixture.CreateAuthenticatedHttpClientAsync(Constants.AiInformationExtractionServiceName);
        using var content = new MultipartFormDataContent();
        const string fileName = "scratch-rear-left-fender.jpeg";
        await using var imageStream = typeof(ImageIntegrationTests).GetEmbeddedStream(fileName);
        using var streamContent = new StreamContent(imageStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(streamContent, "file", fileName);

        var imageId = Guid.NewGuid();
        using var uploadResponse = await httpClient.PutAsync($"/api/media/{imageId}", content);
        uploadResponse.EnsureSuccessStatusCode();

        using var getResponse = await httpClient.GetAsync($"/api/media/{imageId}");
        getResponse.EnsureSuccessStatusCode();
        var downloadedPicture = await getResponse.Content.ReadAsByteArrayAsync();
        imageStream.Position = 0;
        var originalImageArray = new byte[imageStream.Length];
        var readBytes = await imageStream.ReadAsync(originalImageArray.AsMemory());
        readBytes.Should().Be(originalImageArray.Length);
        downloadedPicture.Should().Equal(originalImageArray);

        using var analyzeResponse = await httpClient.PutAsJsonAsync(
            "/api/analyze/image",
            new AnalyzeImageRequestDto(FormSection.VehicleDamage, imageId),
            Shared.JsonAccess.Json.Options
        );
        analyzeResponse.EnsureSuccessStatusCode();
        var analysisResponseDto =
            await analyzeResponse.Content.ReadFromJsonAsync<AnalysisResponseDto>(Shared.JsonAccess.Json.Options);
        var expectedResponseDto = new AnalysisResponseDto(
            AnalysisType.Image,
            FormSection.VehicleDamage,
            new VehicleDamageDto { RearLeftFender = DamageType.Scratch }
        );
        analysisResponseDto.Should().Be(expectedResponseDto);
    }

    // ReSharper disable NotAccessedPositionalProperty.Local -- Accessed by FluentAssertions
    private sealed record AnalyzeImageRequestDto(
        FormSection FormSection,
        Guid ImageId
    );

    private sealed record AnalysisResponseDto(
        AnalysisType AnalysisType,
        FormSection FormSection,
        VehicleDamageDto AnalysisResult
    );
    // ReSharper restore NotAccessedPositionalProperty.Local
}