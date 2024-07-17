using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.JsonAccess;

public static class Json
{
    public static JsonSerializerOptions Options { get; } = new (JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static JsonDocumentOptions DocumentOptions { get; } = new ()
    {
        MaxDepth = 10,
        AllowTrailingCommas = true,
        CommentHandling = JsonCommentHandling.Skip
    };

    public static JsonDocument ParseDocument(string json) => JsonDocument.Parse(json, DocumentOptions);

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public static T Deserialize<T>(string json)
    {
        var returnValue = JsonSerializer.Deserialize<T>(json, Options);
        if (returnValue is null)
        {
            throw new InvalidDataException("Did not expect null JSON document");
        }

        return returnValue;
    }
}