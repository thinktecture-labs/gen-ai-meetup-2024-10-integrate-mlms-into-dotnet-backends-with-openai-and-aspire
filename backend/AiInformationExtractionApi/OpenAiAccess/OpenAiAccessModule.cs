using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;

namespace AiInformationExtractionApi.OpenAiAccess;

public static class OpenAiAccessModule
{
    public static IServiceCollection AddOpenAiAccess(this IServiceCollection services) =>
        services
           .AddSingleton(sp => OpenAiOptions.FromConfiguration(sp.GetRequiredService<IConfiguration>()))
           .AddSingleton(
                sp =>
                {
                    var options = sp.GetRequiredService<OpenAiOptions>();
                    return new OpenAIClient(options.ApiKey);
                }
            )
           .AddSingleton(
                new ChatCompletionOptions
                {
                    Temperature = 0f,
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                }
            )
           .AddScoped<ChatClient>(
                sp =>
                {
                    var options = sp.GetRequiredService<OpenAiOptions>();
                    return sp.GetRequiredService<OpenAIClient>().GetChatClient(options.GptModel);
                }
            )
           .AddScoped<IAiChatClient, AiChatClient>()
           .AddSingleton(
                new AudioTranscriptionOptions
                {
                    Temperature = 0f
                }
            )
           .AddScoped<AudioClient>(
                sp =>
                {
                    var options = sp.GetRequiredService<OpenAiOptions>();
                    return sp.GetRequiredService<OpenAIClient>().GetAudioClient(options.AudioTranscriptionModel);
                }
            )
           .AddScoped<IAiAudioClient, OpenAiAudioClient>();
}