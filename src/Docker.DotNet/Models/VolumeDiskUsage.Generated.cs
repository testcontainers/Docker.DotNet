#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeDiskUsage // (volume.DiskUsage)
    {
        [JsonPropertyName("ActiveCount")]
        public long? ActiveCount { get; set; }

        [JsonPropertyName("Items")]
        public IList<Volume>? Items { get; set; }

        [JsonPropertyName("Reclaimable")]
        public long? Reclaimable { get; set; }

        [JsonPropertyName("TotalCount")]
        public long? TotalCount { get; set; }

        [JsonPropertyName("TotalSize")]
        public long? TotalSize { get; set; }
    }
}
