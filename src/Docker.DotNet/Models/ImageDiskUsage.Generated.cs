#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageDiskUsage // (image.DiskUsage)
    {
        [JsonPropertyName("ActiveCount")]
        public long? ActiveCount { get; set; }

        [JsonPropertyName("Items")]
        public IList<ImagesListResponse>? Items { get; set; }

        [JsonPropertyName("Reclaimable")]
        public long? Reclaimable { get; set; }

        [JsonPropertyName("TotalCount")]
        public long? TotalCount { get; set; }

        [JsonPropertyName("TotalSize")]
        public long? TotalSize { get; set; }
    }
}
