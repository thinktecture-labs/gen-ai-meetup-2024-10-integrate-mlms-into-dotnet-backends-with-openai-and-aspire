using System;
using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.DatabaseAccess.Model;

namespace AiInformationExtractionApi.Media.GetMedia;

public interface IGetMediaDbSession
{
    Task<MediaItem?> GetMediaItemAsync(Guid id, CancellationToken cancellationToken = default);
}