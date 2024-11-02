using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Shared;
using Shared.Messages.Analyze;
using Shared.Messages.DamageReports;
using Xunit;

namespace AspireAppHost.IntegrationTests;

[Collection(nameof(AspireCollection))]
public sealed class TextIntegrationTests
{
    private readonly AspireFixture _fixture;

    public TextIntegrationTests(AspireFixture fixture) => _fixture = fixture;

    [SkippableFact]
    public async Task AnalyzeText()
    {
        AspireFixture.SkipTestIfNecessary();

        using var httpClient =
            await _fixture.CreateAuthenticatedHttpClientAsync(Constants.AiInformationExtractionServiceName);
        using var response = await httpClient.PutAsJsonAsync(
            "/api/analyze/text",
            new AnalyzeTextRequestDto(
                FormSection.PersonalData,
                """
                Greetings, my name is Anna Smith, I reside at 102 Oak Avenue, 75001 Uptown, my insurance ID is AB-1234567,
                I was born on 11/15/1985, my phone number is +44 2071234567, my email is anna.smith@example.com,
                and my license plate is XYZ-9876.
                """
            )
        );

        response.EnsureSuccessStatusCode();
        var analysisResponse =
            await response.Content.ReadFromJsonAsync<AnalysisResponseDto>(Shared.JsonAccess.Json.Options);
        var expectedResponse = new AnalysisResponseDto(
            AnalysisType.Text,
            FormSection.PersonalData,
            new PersonalDataDto
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
                InsuranceId = "AB-1234567",
            }
        );
        analysisResponse.Should().Be(expectedResponse);
    }

    // ReSharper disable NotAccessedPositionalProperty.Local -- Accessed by FluentAssertions
    private sealed record AnalyzeTextRequestDto(FormSection FormSection, string TextToAnalyze);

    private sealed record AnalysisResponseDto(
        AnalysisType AnalysisType,
        FormSection FormSection,
        PersonalDataDto AnalysisResult
    );
    // ReSharper restore NotAccessedPositionalProperty.Local
}