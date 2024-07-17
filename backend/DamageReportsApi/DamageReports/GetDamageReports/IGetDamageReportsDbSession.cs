using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Light.SharedCore.DatabaseAccessAbstractions;
using Shared.Messages.DamageReports;

namespace DamageReportsApi.DamageReports.GetDamageReports;

public interface IGetDamageReportsDbSession : IAsyncReadOnlySession
{
    Task<List<DamageReportListDto>> GetDamageReportsAsync(
        int skip,
        int take,
        CancellationToken cancellationToken = default
    );
}