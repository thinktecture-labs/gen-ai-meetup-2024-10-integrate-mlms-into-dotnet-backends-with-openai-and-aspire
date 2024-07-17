using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared;

namespace AiInformationExtractionApi.Media.GetMedia;

public static class GetMediaEndpoint
{
    public static void MapGetMediaEndpoint(this WebApplication app) =>
        app.MapGet("/api/media/{id:guid}", GetMedia)
           .WithName("GetMedia")
           .WithTags(Constants.Media)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization()
           .WithOpenApi();

    private static async Task<IResult> GetMedia(
        Guid id,
        IGetMediaDbSession dbSession,
        CancellationToken cancellationToken = default
    )
    {
        var mediaItem = await dbSession.GetMediaItemAsync(id, cancellationToken);
        return mediaItem is null ?
            TypedResults.NotFound() :
            TypedResults.File(mediaItem.Content, mediaItem.MimeType);
    }
}