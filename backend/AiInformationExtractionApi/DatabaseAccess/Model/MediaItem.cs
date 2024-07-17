namespace AiInformationExtractionApi.DatabaseAccess.Model;

public sealed class MediaItem : BaseMediaItem
{
    public byte[] Content { get; set; } = null!;
}