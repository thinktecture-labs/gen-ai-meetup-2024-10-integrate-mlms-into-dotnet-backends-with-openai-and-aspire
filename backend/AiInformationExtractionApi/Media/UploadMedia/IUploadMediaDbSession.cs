using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Light.SharedCore.DatabaseAccessAbstractions;

namespace AiInformationExtractionApi.Media.UploadMedia;

public interface IUploadMediaDbSession : IAsyncReadOnlySession
{
    Task UploadMediaItemAsync(
        Guid id,
        string name,
        string mimeType,
        Stream content,
        CancellationToken cancellationToken
    );
}