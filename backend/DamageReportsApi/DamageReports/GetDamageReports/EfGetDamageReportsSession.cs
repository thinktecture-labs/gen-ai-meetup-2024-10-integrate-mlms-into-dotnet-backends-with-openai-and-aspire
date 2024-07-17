using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DamageReportsApi.DatabaseAccess;
using Light.DatabaseAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DamageReports.GetDamageReports;

public sealed class EfGetDamageReportsSession : EfAsyncReadOnlySession<DamageReportsDbContext>, IGetDamageReportsDbSession
{
    public EfGetDamageReportsSession(DamageReportsDbContext dbContext) : base(dbContext) { }

    public Task<List<DamageReportListDto>> GetDamageReportsAsync(
        int skip,
        int take,
        CancellationToken cancellationToken = default
    ) =>
        DbContext
           .DamageReports
           .OrderByDescending(report => report.CreatedAtUtc)
           .Skip(skip)
           .Take(take)
           .Select(
                report => new DamageReportListDto(
                    report.Id,
                    report.FirstName,
                    report.LastName,
                    report.DateOfAccidentUtc,
                    report.AccidentType
                )
            )
           .ToListAsync(cancellationToken);
}