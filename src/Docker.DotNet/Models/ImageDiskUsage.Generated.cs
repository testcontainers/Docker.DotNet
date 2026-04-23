#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DiskUsage represents system data usage for image resources.
    /// 
    /// swagger:model DiskUsage
    /// </summary>
    public class ImageDiskUsage // (image.DiskUsage)
    {
        /// <summary>
        /// Count of active images.
        /// 
        /// Example: 1
        /// </summary>
        [JsonPropertyName("ActiveCount")]
        public long? ActiveCount { get; set; }

        /// <summary>
        /// List of image summaries.
        /// </summary>
        [JsonPropertyName("Items")]
        public IList<ImagesListResponse>? Items { get; set; }

        /// <summary>
        /// Disk space that can be reclaimed by removing unused images.
        /// 
        /// Example: 12345678
        /// </summary>
        [JsonPropertyName("Reclaimable")]
        public long? Reclaimable { get; set; }

        /// <summary>
        /// Count of all images.
        /// 
        /// Example: 4
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public long? TotalCount { get; set; }

        /// <summary>
        /// Disk space in use by images.
        /// 
        /// Example: 98765432
        /// </summary>
        [JsonPropertyName("TotalSize")]
        public long? TotalSize { get; set; }
    }
}
