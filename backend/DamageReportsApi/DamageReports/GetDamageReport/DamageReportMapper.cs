using System.Linq;
using DamageReportsApi.DatabaseAccess.Model;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DamageReports.GetDamageReport;

public static class DamageReportMapper
{
    public static DamageReportDto MapToDamageReportDto(this DamageReport damageReport) =>
        new ()
        {
            Id = damageReport.Id,
            CreatedAtUtc = damageReport.CreatedAtUtc,
            PersonalData = new PersonalDataDto
            {
                FirstName = damageReport.FirstName,
                LastName = damageReport.LastName,
                Street = damageReport.Street,
                ZipCode = damageReport.ZipCode,
                Location = damageReport.Location,
                InsuranceId = damageReport.InsuranceId,
                DateOfBirth = damageReport.DateOfBirth,
                Telephone = damageReport.Telephone,
                Email = damageReport.Email,
                LicensePlate = damageReport.LicensePlate
            },
            Circumstances = new CircumstancesDto
            {
                DateOfAccidentUtc = damageReport.DateOfAccidentUtc,
                AccidentType = damageReport.AccidentType,
                Passengers = damageReport.Passengers.Select(
                    passenger => new PersonDto
                    {
                        FirstName = passenger.FirstName,
                        LastName = passenger.LastName
                    }
                ).ToList(),
                CountryCode = damageReport.CountryCode,
                ReasonOfTravel = damageReport.ReasonOfTravel,
                OtherPartyContact = damageReport.OtherPartyContact == null ?
                    null :
                    new PersonDto
                    {
                        FirstName = damageReport.OtherPartyContact.FirstName,
                        LastName = damageReport.OtherPartyContact.LastName
                    },
                CarType = damageReport.CarType,
                CarColor = damageReport.CarColor
            },
            VehicleDamage = new VehicleDamageDto
            {
                FrontBumper = damageReport.FrontBumper,
                RearBumper = damageReport.RearBumper,
                Hood = damageReport.Hood,
                TrunkLid = damageReport.TrunkLid,
                Roof = damageReport.Roof,
                FrontLeftDoor = damageReport.FrontLeftDoor,
                FrontRightDoor = damageReport.FrontRightDoor,
                RearLeftDoor = damageReport.RearLeftDoor,
                RearRightDoor = damageReport.RearRightDoor,
                FrontLeftFender = damageReport.FrontLeftFender,
                FrontRightFender = damageReport.FrontRightFender,
                RearLeftFender = damageReport.RearLeftFender,
                RearRightFender = damageReport.RearRightFender,
                Grille = damageReport.Grille,
                LeftHeadlights = damageReport.LeftHeadlights,
                RightHeadlights = damageReport.RightHeadlights,
                LeftTaillights = damageReport.LeftTaillights,
                RightTaillights = damageReport.RightTaillights,
                LeftSideMirror = damageReport.LeftSideMirror,
                RightSideMirror = damageReport.RightSideMirror,
                Windshield = damageReport.Windshield,
                RearWindshield = damageReport.RearWindshield,
                FrontLeftWindow = damageReport.FrontLeftWindow,
                FrontRightWindow = damageReport.FrontRightWindow,
                RearLeftWindow = damageReport.RearLeftWindow,
                RearRightWindow = damageReport.RearRightWindow,
                FrontLeftWheel = damageReport.FrontLeftWheel,
                FrontRightWheel = damageReport.FrontRightWheel,
                RearLeftWheel = damageReport.RearLeftWheel,
                RearRightWheel = damageReport.RearRightWheel,
                LeftExteriorTrim = damageReport.LeftExteriorTrim,
                RightExteriorTrim = damageReport.RightExteriorTrim
            }
        };

    public static IQueryable<DamageReportDto> MapToDamageReportDto(this IQueryable<DamageReport> damageReports) =>
        damageReports.Select(
            damageReport => new DamageReportDto
            {
                Id = damageReport.Id,
                CreatedAtUtc = damageReport.CreatedAtUtc,
                PersonalData = new PersonalDataDto
                {
                    FirstName = damageReport.FirstName,
                    LastName = damageReport.LastName,
                    Street = damageReport.Street,
                    ZipCode = damageReport.ZipCode,
                    Location = damageReport.Location,
                    InsuranceId = damageReport.InsuranceId,
                    DateOfBirth = damageReport.DateOfBirth,
                    Telephone = damageReport.Telephone,
                    Email = damageReport.Email,
                    LicensePlate = damageReport.LicensePlate
                },
                Circumstances = new CircumstancesDto
                {
                    DateOfAccidentUtc = damageReport.DateOfAccidentUtc,
                    AccidentType = damageReport.AccidentType,
                    Passengers = damageReport.Passengers.Select(
                        passenger => new PersonDto
                        {
                            FirstName = passenger.FirstName,
                            LastName = passenger.LastName
                        }
                    ).ToList(),
                    CountryCode = damageReport.CountryCode,
                    ReasonOfTravel = damageReport.ReasonOfTravel,
                    OtherPartyContact = damageReport.OtherPartyContact == null ?
                        null :
                        new PersonDto
                        {
                            FirstName = damageReport.OtherPartyContact.FirstName,
                            LastName = damageReport.OtherPartyContact.LastName
                        },
                    CarType = damageReport.CarType,
                    CarColor = damageReport.CarColor
                },
                VehicleDamage = new VehicleDamageDto
                {
                    FrontBumper = damageReport.FrontBumper,
                    RearBumper = damageReport.RearBumper,
                    Hood = damageReport.Hood,
                    TrunkLid = damageReport.TrunkLid,
                    Roof = damageReport.Roof,
                    FrontLeftDoor = damageReport.FrontLeftDoor,
                    FrontRightDoor = damageReport.FrontRightDoor,
                    RearLeftDoor = damageReport.RearLeftDoor,
                    RearRightDoor = damageReport.RearRightDoor,
                    FrontLeftFender = damageReport.FrontLeftFender,
                    FrontRightFender = damageReport.FrontRightFender,
                    RearLeftFender = damageReport.RearLeftFender,
                    RearRightFender = damageReport.RearRightFender,
                    Grille = damageReport.Grille,
                    LeftHeadlights = damageReport.LeftHeadlights,
                    RightHeadlights = damageReport.RightHeadlights,
                    LeftTaillights = damageReport.LeftTaillights,
                    RightTaillights = damageReport.RightTaillights,
                    LeftSideMirror = damageReport.LeftSideMirror,
                    RightSideMirror = damageReport.RightSideMirror,
                    Windshield = damageReport.Windshield,
                    RearWindshield = damageReport.RearWindshield,
                    FrontLeftWindow = damageReport.FrontLeftWindow,
                    FrontRightWindow = damageReport.FrontRightWindow,
                    RearLeftWindow = damageReport.RearLeftWindow,
                    RearRightWindow = damageReport.RearRightWindow,
                    FrontLeftWheel = damageReport.FrontLeftWheel,
                    FrontRightWheel = damageReport.FrontRightWheel,
                    RearLeftWheel = damageReport.RearLeftWheel,
                    RearRightWheel = damageReport.RearRightWheel,
                    LeftExteriorTrim = damageReport.LeftExteriorTrim,
                    RightExteriorTrim = damageReport.RightExteriorTrim
                }
            }
        );
}