using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace AiInformationExtractionApi.OpenAiAccess;

public interface IAiChatClient
{
    Task<ChatCompletion> CompleteChatAsync(
        List<ChatMessage> chatMessages,
        CancellationToken cancellationToken = default
    );
}