#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// StatsResponse aggregates all types of stats of one container.
    /// </summary>
    public class ContainerStatsResponse // (container.StatsResponse)
    {
        /// <summary>
        /// ID is the ID of the container for which the stats were collected.
        /// </summary>
        [JsonPropertyName("id")]
        public string? ID { get; set; }

        /// <summary>
        /// Name is the name of the container for which the stats were collected.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// OSType is the OS of the container (&quot;linux&quot; or &quot;windows&quot;) to allow
        /// platform-specific handling of stats.
        /// </summary>
        [JsonPropertyName("os_type")]
        public string? OSType { get; set; }

        /// <summary>
        /// Read is the date and time at which this sample was collected.
        /// </summary>
        [JsonPropertyName("read")]
        public DateTime Read { get; set; } = default!;

        /// <summary>
        /// CPUStats contains CPU related info of the container.
        /// </summary>
        [JsonPropertyName("cpu_stats")]
        public CPUStats? CPUStats { get; set; }

        /// <summary>
        /// MemoryStats aggregates all memory stats since container inception on Linux.
        /// Windows returns stats for commit and private working set only.
        /// </summary>
        [JsonPropertyName("memory_stats")]
        public MemoryStats? MemoryStats { get; set; }

        /// <summary>
        /// Networks contains Nntwork statistics for the container per interface.
        /// 
        /// This field is omitted if the container has no networking enabled.
        /// </summary>
        [JsonPropertyName("networks")]
        public IDictionary<string, NetworkStats>? Networks { get; set; }

        /// <summary>
        /// PidsStats contains Linux-specific stats of a container&apos;s process-IDs (PIDs).
        /// 
        /// This field is Linux-specific and omitted for Windows containers.
        /// </summary>
        [JsonPropertyName("pids_stats")]
        public PidsStats? PidsStats { get; set; }

        /// <summary>
        /// BlkioStats stores all IO service stats for data read and write.
        /// 
        /// This type is Linux-specific and holds many fields that are specific
        /// to cgroups v1.
        /// 
        /// On a cgroup v2 host, all fields other than &quot;io_service_bytes_recursive&quot;
        /// are omitted or &quot;null&quot;.
        /// 
        /// This type is only populated on Linux and omitted for Windows containers.
        /// </summary>
        [JsonPropertyName("blkio_stats")]
        public BlkioStats? BlkioStats { get; set; }

        /// <summary>
        /// NumProcs is the number of processors on the system.
        /// 
        /// This field is Windows-specific and always zero for Linux containers.
        /// </summary>
        [JsonPropertyName("num_procs")]
        public uint NumProcs { get; set; } = default!;

        /// <summary>
        /// StorageStats is the disk I/O stats for read/write on Windows.
        /// 
        /// This type is Windows-specific and omitted for Linux containers.
        /// </summary>
        [JsonPropertyName("storage_stats")]
        public StorageStats? StorageStats { get; set; }

        /// <summary>
        /// PreRead is the date and time at which this first sample was collected.
        /// This field is not propagated if the &quot;one-shot&quot; option is set. If the
        /// &quot;one-shot&quot; option is set, this field may be omitted, empty, or set
        /// to a default date (`0001-01-01T00:00:00Z`).
        /// </summary>
        [JsonPropertyName("preread")]
        public DateTime PreRead { get; set; } = default!;

        /// <summary>
        /// PreCPUStats contains the CPUStats of the previous sample.
        /// </summary>
        [JsonPropertyName("precpu_stats")]
        public CPUStats? PreCPUStats { get; set; }
    }
}
