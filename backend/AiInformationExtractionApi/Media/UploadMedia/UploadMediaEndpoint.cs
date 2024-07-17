using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared;

namespace AiInformationExtractionApi.Media.UploadMedia;

public static class UploadMediaEndpoint
{
    public static void MapUploadMediaEndpoint(this WebApplication app) =>
        app.MapPut("/api/media/{id:guid}", UploadMedia)
           .WithName("UploadMedia")
           .WithTags(Constants.Media)
           .Produces(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status400BadRequest)
           .RequireAuthorization()
           .DisableAntiforgery()
           .WithOpenApi();

    private static async Task<IResult> UploadMedia(
        Guid id,
        IFormFile file,
        IUploadMediaDbSession dbSession,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = file.OpenReadStream();
        await dbSession.UploadMediaItemAsync(
            id,
            file.FileName,
            file.ContentType,
            stream,
            cancellationToken
        );
        return TypedResults.Created("/api/media/" + id);
    }
}