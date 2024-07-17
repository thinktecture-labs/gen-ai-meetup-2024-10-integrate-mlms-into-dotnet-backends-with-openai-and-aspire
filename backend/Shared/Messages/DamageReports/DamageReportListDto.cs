using System;

namespace Shared.Messages.DamageReports;

public sealed record DamageReportListDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime DateOfAccidentUtc,
    AccidentType AccidentType
);