#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DiskUsage represents system data usage information for container resources.
    /// 
    /// swagger:model DiskUsage
    /// </summary>
    public class ContainerDiskUsage // (container.DiskUsage)
    {
        /// <summary>
        /// Count of active containers.
        /// 
        /// Example: 1
        /// </summary>
        [JsonPropertyName("ActiveCount")]
        public long? ActiveCount { get; set; }

        /// <summary>
        /// List of container summaries.
        /// </summary>
        [JsonPropertyName("Items")]
        public IList<ContainerListResponse>? Items { get; set; }

        /// <summary>
        /// Disk space that can be reclaimed by removing inactive containers.
        /// 
        /// Example: 12345678
        /// </summary>
        [JsonPropertyName("Reclaimable")]
        public long? Reclaimable { get; set; }

        /// <summary>
        /// Count of all containers.
        /// 
        /// Example: 4
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public long? TotalCount { get; set; }

        /// <summary>
        /// Disk space in use by containers.
        /// 
        /// Example: 98765432
        /// </summary>
        [JsonPropertyName("TotalSize")]
        public long? TotalSize { get; set; }
    }
}
