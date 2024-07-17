using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DamageReportsApi.DamageReports.GetDamageReport;
using DamageReportsApi.DatabaseAccess.Model;
using Light.SharedCore.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared;
using Shared.Messages.DamageReports;
using Shared.Validation;

namespace DamageReportsApi.DamageReports.SubmitDamageReport;

public static class SubmitDamageReportEndpoint
{
    public static void MapSubmitDamageReport(this WebApplication app) =>
        app.MapPut("/api/damage-reports", SubmitDamageReport)
           .WithName("SubmitDamageReport")
           .WithTags(Constants.DamageReporting)
           .Produces<DamageReportDto>()
           .Produces<DamageReportDto>(StatusCodes.Status201Created)
           .ProducesValidationProblem()
           .Produces(StatusCodes.Status500InternalServerError)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> SubmitDamageReport(
        SubmitDamageReportDto dto,
        SubmitDamageReportDtoValidator validator,
        ISubmitDamageReportDbSession dbSession,
        IClock clock,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        if (validator.CheckForErrors(dto, out var validationProblem))
        {
            return validationProblem;
        }

        var damageReport = await dbSession.GetDamageReportAsync(dto.Id, cancellationToken);
        var isNewDamageReport = false;
        if (damageReport is null)
        {
            damageReport = new DamageReport { Id = dto.Id, CreatedAtUtc = clock.GetTime() };
            await dbSession.AddDamageReportAsync(damageReport, cancellationToken);
            isNewDamageReport = true;
        }

        await BindAsync(dto, damageReport, dbSession, cancellationToken);
        await dbSession.SaveChangesAsync(cancellationToken);
        var damageReportDto = damageReport.MapToDamageReportDto();
        if (isNewDamageReport)
        {
            logger.Information("Created damage report with id {Id}", damageReport.Id);
            return TypedResults.Created($"/api/damage-reports/{damageReport.Id}", damageReportDto);
        }

        logger.Information("Updated damage report with id {Id}", damageReport.Id);
        return TypedResults.Ok(damageReportDto);
    }

    private static async Task BindAsync(
        SubmitDamageReportDto dto,
        DamageReport damageReport,
        ISubmitDamageReportDbSession dbSession,
        CancellationToken cancellationToken
    )
    {
        damageReport.FirstName = dto.PersonalData.FirstName;
        damageReport.LastName = dto.PersonalData.LastName;
        damageReport.Street = dto.PersonalData.Street;
        damageReport.ZipCode = dto.PersonalData.ZipCode;
        damageReport.Location = dto.PersonalData.Location;
        damageReport.InsuranceId = dto.PersonalData.InsuranceId;
        damageReport.DateOfBirth = dto.PersonalData.DateOfBirth;
        damageReport.Telephone = dto.PersonalData.Telephone;
        damageReport.Email = dto.PersonalData.Email;
        damageReport.LicensePlate = dto.PersonalData.LicensePlate;

        damageReport.DateOfAccidentUtc = dto.Circumstances.DateOfAccidentUtc;
        damageReport.AccidentType = dto.Circumstances.AccidentType;
        await BindPassengersAsync(dto.Circumstances.Passengers, damageReport, dbSession, cancellationToken);
        damageReport.CountryCode = dto.Circumstances.CountryCode;
        damageReport.ReasonOfTravel = dto.Circumstances.ReasonOfTravel;
        await BindOtherPartyContactAsync(
            dto.Circumstances.OtherPartyContact,
            damageReport,
            dbSession,
            cancellationToken
        );
        damageReport.CarType = dto.Circumstances.CarType;
        damageReport.CarColor = dto.Circumstances.CarColor;

        damageReport.FrontBumper = dto.VehicleDamage.FrontBumper;
        damageReport.RearBumper = dto.VehicleDamage.RearBumper;
        damageReport.Hood = dto.VehicleDamage.Hood;
        damageReport.TrunkLid = dto.VehicleDamage.TrunkLid;
        damageReport.Roof = dto.VehicleDamage.Roof;
        damageReport.FrontLeftDoor = dto.VehicleDamage.FrontLeftDoor;
        damageReport.FrontRightDoor = dto.VehicleDamage.FrontRightDoor;
        damageReport.RearLeftDoor = dto.VehicleDamage.RearLeftDoor;
        damageReport.RearRightDoor = dto.VehicleDamage.RearRightDoor;
        damageReport.FrontLeftFender = dto.VehicleDamage.FrontLeftFender;
        damageReport.FrontRightFender = dto.VehicleDamage.FrontRightFender;
        damageReport.RearLeftFender = dto.VehicleDamage.RearLeftFender;
        damageReport.RearRightFender = dto.VehicleDamage.RearRightFender;
        damageReport.Grille = dto.VehicleDamage.Grille;
        damageReport.LeftHeadlights = dto.VehicleDamage.LeftHeadlights;
        damageReport.RightHeadlights = dto.VehicleDamage.RightHeadlights;
        damageReport.LeftTaillights = dto.VehicleDamage.LeftTaillights;
        damageReport.RightTaillights = dto.VehicleDamage.RightTaillights;
        damageReport.LeftSideMirror = dto.VehicleDamage.LeftSideMirror;
        damageReport.RightSideMirror = dto.VehicleDamage.RightSideMirror;
        damageReport.Windshield = dto.VehicleDamage.Windshield;
        damageReport.RearWindshield = dto.VehicleDamage.RearWindshield;
        damageReport.FrontLeftWindow = dto.VehicleDamage.FrontLeftWindow;
        damageReport.FrontRightWindow = dto.VehicleDamage.FrontRightWindow;
        damageReport.RearLeftWindow = dto.VehicleDamage.RearLeftWindow;
        damageReport.RearRightWindow = dto.VehicleDamage.RearRightWindow;
        damageReport.FrontLeftWheel = dto.VehicleDamage.FrontLeftWheel;
        damageReport.FrontRightWheel = dto.VehicleDamage.FrontRightWheel;
        damageReport.RearLeftWheel = dto.VehicleDamage.RearLeftWheel;
        damageReport.RearRightWheel = dto.VehicleDamage.RearRightWheel;
        damageReport.LeftExteriorTrim = dto.VehicleDamage.LeftExteriorTrim;
        damageReport.RightExteriorTrim = dto.VehicleDamage.RightExteriorTrim;
    }

    private static async Task BindPassengersAsync(
        List<PersonDto> passengerDtoList,
        DamageReport damageReport,
        ISubmitDamageReportDbSession dbSession,
        CancellationToken cancellationToken
    )
    {
        var passengers = damageReport.Passengers;
        for (var i = 0; i < passengerDtoList.Count; i++)
        {
            var passengerDto = passengerDtoList[i];
            if (passengers.Count <= i)
            {
                var newPassenger = new Passenger { Id = Guid.NewGuid(), DamageReportId = damageReport.Id };
                await dbSession.AddPassengerAsync(newPassenger, cancellationToken);
                passengers.Add(newPassenger);
            }

            var passenger = passengers[i];
            passenger.FirstName = passengerDto.FirstName;
            passenger.LastName = passengerDto.LastName;
        }

        while (passengers.Count > passengerDtoList.Count)
        {
            await dbSession.RemovePassengerAsync(passengers[^1], cancellationToken);
            passengers.RemoveAt(passengers.Count - 1);
        }
    }

    private static async Task BindOtherPartyContactAsync(
        PersonDto? otherPartyContactDto,
        DamageReport damageReport,
        ISubmitDamageReportDbSession dbSession,
        CancellationToken cancellationToken
    )
    {
        if (otherPartyContactDto is null)
        {
            if (damageReport.OtherPartyContact is not null)
            {
                await dbSession.RemoveOtherPartyContactAsync(damageReport.OtherPartyContact, cancellationToken);
                damageReport.OtherPartyContact = null;
            }

            return;
        }

        var otherPartyContact = damageReport.OtherPartyContact;
        if (otherPartyContact is null)
        {
            otherPartyContact = new OtherPartyContact { Id = Guid.NewGuid(), DamageReportId = damageReport.Id };
            await dbSession.AddOtherPartyContactAsync(otherPartyContact, cancellationToken);
            damageReport.OtherPartyContact = otherPartyContact;
        }

        otherPartyContact.FirstName = otherPartyContactDto.FirstName;
        otherPartyContact.LastName = otherPartyContactDto.LastName;
    }
}