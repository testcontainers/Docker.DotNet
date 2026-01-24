namespace Docker.DotNet.Models
{
    public class ImagesPruneResponse // (image.PruneReport)
    {
        [JsonPropertyName("ImagesDeleted")]
        public IList<ImageDeleteResponse> ImagesDeleted { get; set; }

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; }
    }
}
