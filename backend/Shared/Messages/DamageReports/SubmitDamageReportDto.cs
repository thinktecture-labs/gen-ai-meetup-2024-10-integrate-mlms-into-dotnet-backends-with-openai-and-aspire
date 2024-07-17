using System;

namespace Shared.Messages.DamageReports;

public sealed record SubmitDamageReportDto
{
    public required Guid Id { get; init; }
    public required PersonalDataDto PersonalData { get; init; }
    public required CircumstancesDto Circumstances { get; init; }
    public required VehicleDamageDto VehicleDamage { get; init; }
}