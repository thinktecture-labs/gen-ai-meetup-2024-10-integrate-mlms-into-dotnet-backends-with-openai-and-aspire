using System;
using System.Text.Json;

namespace Shared.Messages.Analyze;

public sealed record AnalyzeImageRequestDto(
    FormSection FormSection,
    Guid ImageId,
    JsonElement ExistingDamageReportData
);