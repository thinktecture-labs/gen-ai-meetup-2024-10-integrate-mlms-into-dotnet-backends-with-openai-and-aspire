namespace Shared.Messages.DamageReports;

public sealed record PersonDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}