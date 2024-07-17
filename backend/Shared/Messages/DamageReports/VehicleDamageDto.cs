namespace Shared.Messages.DamageReports;

public sealed record VehicleDamageDto
{
    public DamageType FrontBumper { get; init; }
    public DamageType RearBumper { get; init; }
    public DamageType Hood { get; init; }
    public DamageType TrunkLid { get; init; }
    public DamageType Roof { get; init; }
    public DamageType FrontLeftDoor { get; init; }
    public DamageType FrontRightDoor { get; init; }
    public DamageType RearLeftDoor { get; init; }
    public DamageType RearRightDoor { get; init; }
    public DamageType FrontLeftFender { get; init; }
    public DamageType FrontRightFender { get; init; }
    public DamageType RearLeftFender { get; init; }
    public DamageType RearRightFender { get; init; }
    public DamageType Grille { get; init; }
    public DamageType LeftHeadlights { get; init; }
    public DamageType RightHeadlights { get; init; }
    public DamageType LeftTaillights { get; init; }
    public DamageType RightTaillights { get; init; }
    public DamageType LeftSideMirror { get; init; }
    public DamageType RightSideMirror { get; init; }
    public DamageType Windshield { get; init; }
    public DamageType RearWindshield { get; init; }
    public DamageType FrontLeftWindow { get; init; }
    public DamageType FrontRightWindow { get; init; }
    public DamageType RearLeftWindow { get; init; }
    public DamageType RearRightWindow { get; init; }
    public DamageType FrontLeftWheel { get; init; }
    public DamageType FrontRightWheel { get; init; }
    public DamageType RearLeftWheel { get; init; }
    public DamageType RearRightWheel { get; init; }
    public DamageType LeftExteriorTrim { get; init; }
    public DamageType RightExteriorTrim { get; init; }
}