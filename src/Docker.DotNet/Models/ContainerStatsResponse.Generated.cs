namespace Docker.DotNet.Models
{
    public class ContainerStatsResponse // (container.StatsResponse)
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("os_type")]
        public string OSType { get; set; }

        [JsonPropertyName("read")]
        public DateTime Read { get; set; }

        [JsonPropertyName("cpu_stats")]
        public CPUStats CPUStats { get; set; }

        [JsonPropertyName("memory_stats")]
        public MemoryStats MemoryStats { get; set; }

        [JsonPropertyName("networks")]
        public IDictionary<string, NetworkStats> Networks { get; set; }

        [JsonPropertyName("pids_stats")]
        public PidsStats PidsStats { get; set; }

        [JsonPropertyName("blkio_stats")]
        public BlkioStats BlkioStats { get; set; }

        [JsonPropertyName("num_procs")]
        public uint NumProcs { get; set; }

        [JsonPropertyName("storage_stats")]
        public StorageStats StorageStats { get; set; }

        [JsonPropertyName("preread")]
        public DateTime PreRead { get; set; }

        [JsonPropertyName("precpu_stats")]
        public CPUStats PreCPUStats { get; set; }
    }
}
