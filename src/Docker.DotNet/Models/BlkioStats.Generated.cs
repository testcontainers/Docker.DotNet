#nullable enable
namespace Docker.DotNet.Models
{
    public class BlkioStats // (container.BlkioStats)
    {
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
