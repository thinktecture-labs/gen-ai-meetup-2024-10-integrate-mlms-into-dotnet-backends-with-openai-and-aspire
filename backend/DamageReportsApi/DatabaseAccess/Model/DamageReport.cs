using System;
using System.Collections.Generic;
using Light.SharedCore.Entities;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DatabaseAccess.Model;

public sealed class DamageReport : GuidEntity
{
    public DateTime CreatedAtUtc { get; set; }

    // Personal data
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string InsuranceId { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;

    // Circumstances
    public DateTime DateOfAccidentUtc { get; set; }
    public AccidentType AccidentType { get; set; }
    public List<Passenger> Passengers { get; set; } = [];
    public string CountryCode { get; set; } = string.Empty;
    public string ReasonOfTravel { get; set; } = string.Empty;
    public OtherPartyContact? OtherPartyContact { get; set; }
    public string CarType { get; set; } = string.Empty;
    public string CarColor { get; set; } = string.Empty;

    // Damage
    public DamageType FrontBumper { get; set; }
    public DamageType RearBumper { get; set; }
    public DamageType Hood { get; set; }
    public DamageType TrunkLid { get; set; }
    public DamageType Roof { get; set; }
    public DamageType FrontLeftDoor { get; set; }
    public DamageType FrontRightDoor { get; set; }
    public DamageType RearLeftDoor { get; set; }
    public DamageType RearRightDoor { get; set; }
    public DamageType FrontLeftFender { get; set; }
    public DamageType FrontRightFender { get; set; }
    public DamageType RearLeftFender { get; set; }
    public DamageType RearRightFender { get; set; }
    public DamageType Grille { get; set; }
    public DamageType LeftHeadlights { get; set; }
    public DamageType RightHeadlights { get; set; }
    public DamageType LeftTaillights { get; set; }
    public DamageType RightTaillights { get; set; }
    public DamageType LeftSideMirror { get; set; }
    public DamageType RightSideMirror { get; set; }
    public DamageType Windshield { get; set; }
    public DamageType RearWindshield { get; set; }
    public DamageType FrontLeftWindow { get; set; }
    public DamageType FrontRightWindow { get; set; }
    public DamageType RearLeftWindow { get; set; }
    public DamageType RearRightWindow { get; set; }
    public DamageType FrontLeftWheel { get; set; }
    public DamageType FrontRightWheel { get; set; }
    public DamageType RearLeftWheel { get; set; }
    public DamageType RearRightWheel { get; set; }
    public DamageType LeftExteriorTrim { get; set; }
    public DamageType RightExteriorTrim { get; set; }
}