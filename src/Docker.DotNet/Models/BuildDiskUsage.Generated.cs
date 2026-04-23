#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DiskUsage represents system data usage for build cache resources.
    /// 
    /// swagger:model DiskUsage
    /// </summary>
    public class BuildDiskUsage // (build.DiskUsage)
    {
        /// <summary>
        /// Count of active build cache records.
        /// 
        /// Example: 1
        /// </summary>
        [JsonPropertyName("ActiveCount")]
        public long? ActiveCount { get; set; }

        /// <summary>
        /// List of build cache records.
        /// </summary>
        [JsonPropertyName("Items")]
        public IList<CacheRecord>? Items { get; set; }

        /// <summary>
        /// Disk space that can be reclaimed by removing inactive build cache records.
        /// 
        /// Example: 12345678
        /// </summary>
        [JsonPropertyName("Reclaimable")]
        public long? Reclaimable { get; set; }

        /// <summary>
        /// Count of all build cache records.
        /// 
        /// Example: 4
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public long? TotalCount { get; set; }

        /// <summary>
        /// Disk space in use by build cache records.
        /// 
        /// Example: 98765432
        /// </summary>
        [JsonPropertyName("TotalSize")]
        public long? TotalSize { get; set; }
    }
}
