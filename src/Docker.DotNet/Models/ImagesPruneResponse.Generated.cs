#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PruneReport contains the response for Engine API:
    /// POST &quot;/images/prune&quot;
    /// </summary>
    public class ImagesPruneResponse // (image.PruneReport)
    {
        [JsonPropertyName("ImagesDeleted")]
        public IList<ImageDeleteResponse> ImagesDeleted { get; set; } = default!;

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; } = default!;
    }
}
