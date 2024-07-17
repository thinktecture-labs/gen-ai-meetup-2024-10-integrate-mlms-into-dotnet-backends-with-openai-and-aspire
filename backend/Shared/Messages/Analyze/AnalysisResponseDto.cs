using System;
using System.Text.Json;

namespace Shared.Messages.Analyze;

public sealed record AnalysisResponseDto(
    AnalysisType AnalysisType,
    FormSection FormSection,
    DateTime CreatedAtUtc,
    JsonElement AnalysisResult
);