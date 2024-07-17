using System.Threading.Tasks;

namespace AiInformationExtractionApi.OpenAiAccess;

public interface IAiAudioClient
{
    Task<string> TranscribeAudioAsync(string audioFilePath);
}