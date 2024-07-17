using System;
using System.Text.Json;
using System.Threading.Tasks;
using AiInformationExtractionApi.Analyze.Prompting;
using AiInformationExtractionApi.OpenAiAccess;
using FluentAssertions;
using Light.Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;
using Shared.JsonAccess;
using Shared.Messages.Analyze;
using Shared.Messages.DamageReports;
using Xunit;

namespace AiInformationExtractionApi.Tests;

public sealed class OpenAiIntegrationTests : IAsyncLifetime
{
    private readonly AsyncServiceScope _scope;
    private readonly ServiceProvider _serviceProvider;

    public OpenAiIntegrationTests()
    {
        _serviceProvider = new ServiceCollection()
           .AddSingleton(TestSettings.Configuration)
           .AddOpenAiAccess()
           .AddPromptingModule()
           .BuildServiceProvider();
        _scope = _serviceProvider.CreateAsyncScope();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }

    [SkippableFact]
    public async Task ExtractPersonalDataFromText()
    {
        SkipTestIfNecessary();

        const string textToAnalyze =
            """
            Greetings, my name is Anna Smith, I reside at 102 Oak Avenue, 75001 Uptown, my insurance ID is AB-1234567,
            I was born on 11/15/1985, my phone number is +44 2071234567, my email is anna.smith@example.com,
            and my license plate is XYZ-9876.
            """;
        var promptManager = _scope.ServiceProvider.GetRequiredService<PromptManager>();
        var messages = promptManager.CreateTextAnalysisPrompt(FormSection.PersonalData, textToAnalyze);
        var client = _scope.ServiceProvider.GetRequiredService<IAiChatClient>();

        var response = await client.CompleteChatAsync(messages);

        response.Content.Should().HaveCount(1);
        var textPart = response.Content[0];
        textPart.Kind.Should().Be(ChatMessageContentPartKind.Text);
        var jsonDocument = Json.ParseDocument(textPart.Text);
        var personalDataDto = jsonDocument.Deserialize<PersonalDataDto>(Json.Options);
        var expectedPersonalData = new PersonalDataDto
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
        personalDataDto.Should().Be(expectedPersonalData);
    }

    [SkippableFact]
    public async Task ExtractCircumstancesFromText()
    {
        SkipTestIfNecessary();

        const string textToAnalyze =
            """
            The accident happened on 2023-07-20 at 4 in the afternoon. When I walked out of the city hall, I saw that
            my car, a dark-green Toyota Yaris was damaged. I was alone that day. I don't know which car caused the
            accident, nor the driver's name.
            """;
        var existingInformation = Json.Serialize(
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
                InsuranceId = "AB-1234567"
            }
        );
        var promptManager = _scope.ServiceProvider.GetRequiredService<PromptManager>();
        var chatMessages = promptManager.CreateTextAnalysisPrompt(
            FormSection.Circumstances,
            textToAnalyze,
            existingInformation
        );
        var client = _scope.ServiceProvider.GetRequiredService<IAiChatClient>();

        var response = await client.CompleteChatAsync(chatMessages);

        response.Content.Should().HaveCount(1);
        var textPart = response.Content[0];
        textPart.Kind.Should().Be(ChatMessageContentPartKind.Text);
        var circumstancesDto = Json.Deserialize<CircumstancesDto>(textPart.Text);
        var expectedCircumstances = new CircumstancesDto
        {
            DateOfAccidentUtc = new DateTime(2023, 7, 20, 16, 0, 0, DateTimeKind.Utc),
            AccidentType = AccidentType.CarAccident,
            CarType = "Toyota Yaris",
            CarColor = "Dark-Green",
            Passengers = [new PersonDto { FirstName = "Anna", LastName = "Smith" }]
        };
        circumstancesDto.Should().Be(expectedCircumstances);
    }

    private static void SkipTestIfNecessary() =>
        Skip.IfNot(TestSettings.Configuration.GetValue<bool>("openAi:areTestsEnabled"));
}