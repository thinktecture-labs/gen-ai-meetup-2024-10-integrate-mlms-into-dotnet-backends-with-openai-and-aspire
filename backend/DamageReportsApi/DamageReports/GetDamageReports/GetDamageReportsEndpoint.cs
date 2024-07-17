using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared;
using Shared.Messages.DamageReports;
using Shared.Paging;
using Shared.Validation;

namespace DamageReportsApi.DamageReports.GetDamageReports;

public static class GetDamageReportsEndpoint
{
    public static void MapGetDamageReports(this WebApplication app) =>
        app.MapGet("/api/damage-reports", GetDamageReports)
           .WithName("GetDamageReports")
           .WithTags(Constants.DamageReporting)
           .Produces<List<DamageReportListDto>>()
           .ProducesValidationProblem()
           .Produces(StatusCodes.Status500InternalServerError)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> GetDamageReports(
        IGetDamageReportsDbSession dbSession,
        PagingParametersValidator validator,
        int skip = 0,
        int take = 20,
        CancellationToken cancellationToken = default
    )
    {
        if (validator.CheckForErrors(new PagingParameters(skip, take), out var validationProblem))
        {
            return validationProblem;
        }

        var damageReports = await dbSession.GetDamageReportsAsync(skip, take, cancellationToken);
        return TypedResults.Ok(damageReports);
    }
}