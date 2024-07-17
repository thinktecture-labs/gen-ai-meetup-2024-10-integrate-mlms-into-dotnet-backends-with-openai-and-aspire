using System;
using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.DatabaseAccess;
using AiInformationExtractionApi.DatabaseAccess.Model;
using Light.DatabaseAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AiInformationExtractionApi.Media.GetMedia;

public sealed class EfGetMediaSession : EfAsyncReadOnlySession<AiInformationExtractionDbContext>, IGetMediaDbSession
{
    public EfGetMediaSession(AiInformationExtractionDbContext dbContext) : base(dbContext) { }

    public Task<MediaItem?> GetMediaItemAsync(Guid id, CancellationToken cancellationToken = default) =>
        DbContext.MediaItems.FirstOrDefaultAsync(mi => mi.Id == id, cancellationToken);
}