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
public sealed class SpeechIntegrationTests
{
    private readonly AspireFixture _fixture;

    public SpeechIntegrationTests(AspireFixture fixture) => _fixture = fixture;

    [SkippableFact]
    public async Task AnalyzeSpeech()
    {
        AspireFixture.SkipTestIfNecessary();

        // Act
        using var httpClient =
            await _fixture.CreateAuthenticatedHttpClientAsync(Constants.AiInformationExtractionServiceName);
        using var content = new MultipartFormDataContent();
        var existingInformation = new PersonalDataDto
        {
            FirstName = "Anna",
            LastName = "Smith",
            Street = "102 Oak Avenue",
            ZipCode = "75001",
            Location = "Uptown",
            LicensePlate = "XYZ-9876",
            DateOfBirth = new DateOnly(1985, 11, 15),
            Telephone = "+44 2071234567",
            Email = "anna.smith@example.com",
            InsuranceId = "AB-1234567"
        };
        var analyzeSpeechRequestDto = new AnalyzeSpeechRequestDto(FormSection.Circumstances, existingInformation);
        content.Add(JsonContent.Create(analyzeSpeechRequestDto, options: Shared.JsonAccess.Json.Options));
        await using var mp3Stream =
            typeof(SpeechIntegrationTests).GetEmbeddedStream("vehicle-theft-in-front-of-supermarket.mp3");
        using var streamContent = new StreamContent(mp3Stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
        content.Add(streamContent, "audio", "vehicle-theft-in-front-of-supermarket.mp3");
        using var response = await httpClient.PutAsync("/api/analyze/speech", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AnalysisResponseDto>(Shared.JsonAccess.Json.Options);
        var expectedResponse = new AnalysisResponseDto(
            AnalysisType.Speech,
            FormSection.Circumstances,
            new CircumstancesDto
            {
                AccidentType = AccidentType.VehicleTheft,
                DateOfAccidentUtc = new DateTime(2024, 8, 21, 15, 0, 0),
                CarColor = "Grey",
                CarType = "BMW X3",
                Passengers = [new PersonDto { FirstName = "Anna", LastName = "Smith" }],
                ReasonOfTravel = "Shopping at the supermarket"
            }
        );
        result.Should().Be(expectedResponse);
    }

    // ReSharper disable NotAccessedPositionalProperty.Local -- Accessed by FluentAssertions
    private sealed record AnalyzeSpeechRequestDto(FormSection FormSection, PersonalDataDto ExistingDamageReportData);

    private sealed record AnalysisResponseDto(
        AnalysisType AnalysisType,
        FormSection FormSection,
        CircumstancesDto AnalysisResult
    );
    // ReSharper restore NotAccessedPositionalProperty.Local
}