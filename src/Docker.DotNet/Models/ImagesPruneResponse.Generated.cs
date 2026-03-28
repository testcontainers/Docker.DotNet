#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesPruneResponse // (image.PruneReport)
    {
        [JsonPropertyName("ImagesDeleted")]
        public IList<ImageDeleteResponse> ImagesDeleted { get; set; } = default!;

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; } = default!;
    }
}
