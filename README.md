# Integrate Multimodal Language Models into .NET Backends with OpenAI .NET API and .NET Aspire

This talk was given at the October 2024 [Generation AI Meetup](https://www.meetup.com/de-DE/generation-ai-rhein-neckar-kreis-karlsruhe/events/301303206/) in Karlsruhe, Germany.

## Prerequisites

You require the following prerequisites:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- An [OpenAI API key](https://platform.openai.com/docs/overview)

You can configure the API key by creating a file called `appsettings.Development.json` in the folder `backend/AiInformationExtractionApi` and add the following contents:

```json
{
    "openAi": {
        "apiKey": "paste your OpenAI API key here"
    }
}
```

You can also use [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux) to achieve the same thing.

## How to compile and run the example

Before you can compile the example, you need to download the .NET aspire workload. Execute the following statements in a terminal of your choice.

```bash
dotnet workload update
dotnet workload install aspire
```

Afterwards, you should be able to build and run the solution via your IDE or with `dotnet run`.

The most important integration tests can be found in the `AspireAppHost.IntegrationTests` project:

- TextIntegrationTests
- SpeechIntegrationTests
- ImageIntegrationTests
