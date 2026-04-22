#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CPUStats aggregates and wraps all CPU related info of container
    /// </summary>
    public class CPUStats // (container.CPUStats)
    {
        /// <summary>
        /// CPU Usage. Linux and Windows.
        /// </summary>
        [JsonPropertyName("cpu_usage")]
        public CPUUsage CPUUsage { get; set; } = default!;

        /// <summary>
        /// System Usage. Linux only.
        /// </summary>
        [JsonPropertyName("system_cpu_usage")]
        public ulong? SystemUsage { get; set; }

        /// <summary>
        /// Online CPUs. Linux only.
        /// </summary>
        [JsonPropertyName("online_cpus")]
        public uint? OnlineCPUs { get; set; }

        /// <summary>
        /// Throttling Data. Linux only.
        /// </summary>
        [JsonPropertyName("throttling_data")]
        public ThrottlingData? ThrottlingData { get; set; }
    }
}
