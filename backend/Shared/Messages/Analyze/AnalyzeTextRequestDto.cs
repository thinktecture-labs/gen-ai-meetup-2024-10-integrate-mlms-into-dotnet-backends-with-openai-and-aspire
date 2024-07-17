using System.Text.Json;

namespace Shared.Messages.Analyze;

public sealed record AnalyzeTextRequestDto(
    FormSection FormSection,
    string TextToAnalyze,
    JsonElement ExistingDamageReportData
);