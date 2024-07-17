using System.Threading.Tasks;
using OpenAI.Audio;

namespace AiInformationExtractionApi.OpenAiAccess;

public sealed class OpenAiAudioClient : IAiAudioClient
{
    private readonly AudioClient _audioClient;
    private readonly AudioTranscriptionOptions _options;

    public OpenAiAudioClient(AudioClient audioClient, AudioTranscriptionOptions options)
    {
        _audioClient = audioClient;
        _options = options;
    }

    public async Task<string> TranscribeAudioAsync(string filePath)
    {
        var result = await _audioClient.TranscribeAudioAsync(filePath, _options);
        return result.Value.Text;
    }
}