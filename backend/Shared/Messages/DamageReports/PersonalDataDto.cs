using System;

namespace Shared.Messages.DamageReports;

public sealed record PersonalDataDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string InsuranceId { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public string Telephone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
}