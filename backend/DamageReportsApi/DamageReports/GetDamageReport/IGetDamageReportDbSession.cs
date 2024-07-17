using System;
using System.Threading;
using System.Threading.Tasks;
using Light.SharedCore.DatabaseAccessAbstractions;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DamageReports.GetDamageReport;

public interface IGetDamageReportDbSession : IAsyncReadOnlySession
{
    Task<DamageReportDto?> GetDamageReportAsync(Guid id, CancellationToken cancellationToken = default);
}