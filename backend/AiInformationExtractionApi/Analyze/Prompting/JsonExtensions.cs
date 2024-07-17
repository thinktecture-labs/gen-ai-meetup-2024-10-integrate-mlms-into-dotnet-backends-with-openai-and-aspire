using System.Text.Json;

namespace AiInformationExtractionApi.Analyze.Prompting;

public static class JsonExtensions
{
    public static string? CreateAdditionalPromptInformation(this JsonElement jsonElement) =>
        jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined ?
            null :
            $"""
             These are values in JSON format that I already entered into the form:

             ```json
             {jsonElement}
             ```
             """;
}