using System;
using System.Threading;
using System.Threading.Tasks;
using DamageReportsApi.DatabaseAccess.Model;
using Light.SharedCore.DatabaseAccessAbstractions;

namespace DamageReportsApi.DamageReports.SubmitDamageReport;

public interface ISubmitDamageReportDbSession : IAsyncSession
{
    Task<DamageReport?> GetDamageReportAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddDamageReportAsync(DamageReport damageReport, CancellationToken cancellationToken = default);
    Task AddPassengerAsync(Passenger passenger, CancellationToken cancellationToken = default);
    Task AddOtherPartyContactAsync(OtherPartyContact otherPartyContact, CancellationToken cancellationToken = default);
    Task RemovePassengerAsync(Passenger passenger, CancellationToken cancellationToken = default);

    Task RemoveOtherPartyContactAsync(
        OtherPartyContact otherPartyContact,
        CancellationToken cancellationToken = default
    );
}