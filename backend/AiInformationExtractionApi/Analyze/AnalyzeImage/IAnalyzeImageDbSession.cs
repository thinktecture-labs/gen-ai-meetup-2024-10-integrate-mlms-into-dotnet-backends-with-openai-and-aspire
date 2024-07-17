using System;
using System.Threading;
using System.Threading.Tasks;
using AiInformationExtractionApi.DatabaseAccess.Model;
using Light.SharedCore.DatabaseAccessAbstractions;

namespace AiInformationExtractionApi.Analyze.AnalyzeImage;

public interface IAnalyzeImageDbSession : IAsyncReadOnlySession
{
    Task<MediaItem?> GetMediaItemAsync(Guid id, CancellationToken cancellationToken = default);
}