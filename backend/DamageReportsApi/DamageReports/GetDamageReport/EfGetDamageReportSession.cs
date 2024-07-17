using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DamageReportsApi.DatabaseAccess;
using Light.DatabaseAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DamageReports.GetDamageReport;

public sealed class EfGetDamageReportSession : EfAsyncReadOnlySession<DamageReportsDbContext>, IGetDamageReportDbSession
{
    public EfGetDamageReportSession(DamageReportsDbContext dbContext) : base(dbContext) { }

    public Task<DamageReportDto?> GetDamageReportAsync(Guid id, CancellationToken cancellationToken = default) =>
        DbContext
           .DamageReports
           .Include(report => report.Passengers)
           .Include(report => report.OtherPartyContact)
           .Where(report => report.Id == id)
           .MapToDamageReportDto()
           .FirstOrDefaultAsync(cancellationToken);
}