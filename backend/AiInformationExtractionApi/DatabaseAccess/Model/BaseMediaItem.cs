using Light.SharedCore.Entities;

namespace AiInformationExtractionApi.DatabaseAccess.Model;

public abstract class BaseMediaItem : GuidEntity
{
    public string Name { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public int ContentSizeInBytes { get; set; }
}