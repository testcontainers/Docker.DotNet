#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerStatsResponse // (container.StatsResponse)
    {
        [JsonPropertyName("id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("os_type")]
        public string OSType { get; set; } = default!;

        [JsonPropertyName("read")]
        public DateTime Read { get; set; } = default!;

        [JsonPropertyName("cpu_stats")]
        public CPUStats CPUStats { get; set; } = default!;

        [JsonPropertyName("memory_stats")]
        public MemoryStats MemoryStats { get; set; } = default!;

        [JsonPropertyName("networks")]
        public IDictionary<string, NetworkStats> Networks { get; set; } = default!;

        [JsonPropertyName("pids_stats")]
        public PidsStats PidsStats { get; set; } = default!;

        [JsonPropertyName("blkio_stats")]
        public BlkioStats BlkioStats { get; set; } = default!;

        [JsonPropertyName("num_procs")]
        public uint NumProcs { get; set; } = default!;

        [JsonPropertyName("storage_stats")]
        public StorageStats StorageStats { get; set; } = default!;

        [JsonPropertyName("preread")]
        public DateTime PreRead { get; set; } = default!;

        [JsonPropertyName("precpu_stats")]
        public CPUStats PreCPUStats { get; set; } = default!;
    }
}
