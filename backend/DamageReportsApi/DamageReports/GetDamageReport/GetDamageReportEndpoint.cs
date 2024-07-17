using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DamageReports.GetDamageReport;

public static class GetDamageReportEndpoint
{
    public static void MapGetDamageReport(this WebApplication app) =>
        app.MapGet("/api/damage-reports/{id:guid}", GetDamageReport)
           .WithName("GetDamageReport")
           .WithTags(Constants.DamageReporting)
           .Produces<DamageReportDto>()
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> GetDamageReport(
        IGetDamageReportDbSession dbSession,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var damageReport = await dbSession.GetDamageReportAsync(id, cancellationToken);
        return damageReport is null ? TypedResults.NotFound() : TypedResults.Ok(damageReport);
    }
}