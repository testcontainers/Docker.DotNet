#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// BlkioStats stores All IO service stats for data read and write.
    /// This is a Linux specific structure as the differences between expressing
    /// block I/O on Windows and Linux are sufficiently significant to make
    /// little sense attempting to morph into a combined structure.
    /// </summary>
    public class BlkioStats // (container.BlkioStats)
    {
        /// <summary>
        /// number of bytes transferred to and from the block device
        /// </summary>
        [JsonPropertyName("io_service_bytes_recursive")]
        public IList<BlkioStatEntry> IoServiceBytesRecursive { get; set; } = default!;

        [JsonPropertyName("io_serviced_recursive")]
        public IList<BlkioStatEntry> IoServicedRecursive { get; set; } = default!;

        [JsonPropertyName("io_queue_recursive")]
        public IList<BlkioStatEntry> IoQueuedRecursive { get; set; } = default!;

        [JsonPropertyName("io_service_time_recursive")]
        public IList<BlkioStatEntry> IoServiceTimeRecursive { get; set; } = default!;

        [JsonPropertyName("io_wait_time_recursive")]
        public IList<BlkioStatEntry> IoWaitTimeRecursive { get; set; } = default!;

        [JsonPropertyName("io_merged_recursive")]
        public IList<BlkioStatEntry> IoMergedRecursive { get; set; } = default!;

        [JsonPropertyName("io_time_recursive")]
        public IList<BlkioStatEntry> IoTimeRecursive { get; set; } = default!;

        [JsonPropertyName("sectors_recursive")]
        public IList<BlkioStatEntry> SectorsRecursive { get; set; } = default!;
    }
}
