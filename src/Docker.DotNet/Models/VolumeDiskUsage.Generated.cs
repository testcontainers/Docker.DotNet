#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DiskUsage represents system data usage for volume resources.
    /// 
    /// swagger:model DiskUsage
    /// </summary>
    public class VolumeDiskUsage // (volume.DiskUsage)
    {
        /// <summary>
        /// Count of active volumes.
        /// 
        /// Example: 1
        /// </summary>
        [JsonPropertyName("ActiveCount")]
        public long? ActiveCount { get; set; }

        /// <summary>
        /// List of volumes.
        /// </summary>
        [JsonPropertyName("Items")]
        public IList<Volume>? Items { get; set; }

        /// <summary>
        /// Disk space that can be reclaimed by removing inactive volumes.
        /// 
        /// Example: 12345678
        /// </summary>
        [JsonPropertyName("Reclaimable")]
        public long? Reclaimable { get; set; }

        /// <summary>
        /// Count of all volumes.
        /// 
        /// Example: 4
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public long? TotalCount { get; set; }

        /// <summary>
        /// Disk space in use by volumes.
        /// 
        /// Example: 98765432
        /// </summary>
        [JsonPropertyName("TotalSize")]
        public long? TotalSize { get; set; }
    }
}
