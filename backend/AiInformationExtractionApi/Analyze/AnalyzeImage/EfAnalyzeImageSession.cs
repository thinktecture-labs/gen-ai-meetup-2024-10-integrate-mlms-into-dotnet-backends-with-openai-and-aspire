using System;
using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.DatabaseAccess;
using AiInformationExtractionApi.DatabaseAccess.Model;
using Light.DatabaseAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AiInformationExtractionApi.Analyze.AnalyzeImage;

public sealed class EfAnalyzeImageSession : EfAsyncReadOnlySession<AiInformationExtractionDbContext>,
                                            IAnalyzeImageDbSession
{
    public EfAnalyzeImageSession(AiInformationExtractionDbContext dbContext) : base(dbContext) { }

    public Task<MediaItem?> GetMediaItemAsync(Guid id, CancellationToken cancellationToken = default) =>
        DbContext.MediaItems.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
}