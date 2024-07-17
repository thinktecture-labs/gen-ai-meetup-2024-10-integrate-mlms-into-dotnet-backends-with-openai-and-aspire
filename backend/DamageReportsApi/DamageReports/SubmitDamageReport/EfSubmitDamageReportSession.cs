using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using DamageReportsApi.DatabaseAccess;
using DamageReportsApi.DatabaseAccess.Model;
using Light.DatabaseAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DamageReportsApi.DamageReports.SubmitDamageReport;

public sealed class EfSubmitDamageReportSession : EfAsyncSession<DamageReportsDbContext>.WithTransaction,
                                                  ISubmitDamageReportDbSession
{
    public EfSubmitDamageReportSession(DamageReportsDbContext dbContext) : base(dbContext, IsolationLevel.RepeatableRead) { }

    public async Task<DamageReport?> GetDamageReportAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        return await dbContext
           .DamageReports
           .Include(dr => dr.Passengers)
           .Include(dr => dr.OtherPartyContact)
           .FirstOrDefaultAsync(dr => dr.Id == id, cancellationToken);
    }

    public async Task AddDamageReportAsync(DamageReport damageReport, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        dbContext.DamageReports.Add(damageReport);
    }

    public async Task AddPassengerAsync(Passenger passenger, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        dbContext.Passengers.Add(passenger);
    }

    public async Task AddOtherPartyContactAsync(
        OtherPartyContact otherPartyContact,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        dbContext.OtherPartyContacts.Add(otherPartyContact);
    }

    public async Task RemovePassengerAsync(Passenger passenger, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        dbContext.Passengers.Remove(passenger);
    }

    public async Task RemoveOtherPartyContactAsync(
        OtherPartyContact otherPartyContact,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        dbContext.OtherPartyContacts.Remove(otherPartyContact);
    }
}