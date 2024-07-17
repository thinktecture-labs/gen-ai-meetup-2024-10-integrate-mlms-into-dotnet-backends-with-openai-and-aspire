using System.Text.Json;

namespace Shared.Messages.Analyze;

public sealed record AnalyzeSpeechRequestDto(
    FormSection FormSection,
    JsonElement ExistingDamageReportData
);