using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace AiInformationExtractionApi.OpenAiAccess;

public sealed class AiChatClient : IAiChatClient
{
    private readonly ChatClient _chatClient;
    private readonly ChatCompletionOptions _options;

    public AiChatClient(ChatClient chatClient, ChatCompletionOptions options)
    {
        _chatClient = chatClient;
        _options = options;
    }

    public async Task<ChatCompletion> CompleteChatAsync(
        List<ChatMessage> chatMessages,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _chatClient.CompleteChatAsync(chatMessages, _options, cancellationToken);
        return result.Value;
    }
}