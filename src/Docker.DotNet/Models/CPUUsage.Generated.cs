#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CPUUsage stores All CPU stats aggregated since container inception.
    /// </summary>
    public class CPUUsage // (container.CPUUsage)
    {
        /// <summary>
        /// Total CPU time consumed.
        /// Units: nanoseconds (Linux)
        /// Units: 100&apos;s of nanoseconds (Windows)
        /// </summary>
        [JsonPropertyName("total_usage")]
        public ulong TotalUsage { get; set; } = default!;

        /// <summary>
        /// Total CPU time consumed per core (Linux). Not used on Windows.
        /// Units: nanoseconds.
        /// </summary>
        [JsonPropertyName("percpu_usage")]
        public IList<ulong>? PercpuUsage { get; set; }

        /// <summary>
        /// Time spent by tasks of the cgroup in kernel mode (Linux).
        /// Time spent by all container processes in kernel mode (Windows).
        /// Units: nanoseconds (Linux).
        /// Units: 100&apos;s of nanoseconds (Windows). Not populated for Hyper-V Containers.
        /// </summary>
        [JsonPropertyName("usage_in_kernelmode")]
        public ulong UsageInKernelmode { get; set; } = default!;

        /// <summary>
        /// Time spent by tasks of the cgroup in user mode (Linux).
        /// Time spent by all container processes in user mode (Windows).
        /// Units: nanoseconds (Linux).
        /// Units: 100&apos;s of nanoseconds (Windows). Not populated for Hyper-V Containers
        /// </summary>
        [JsonPropertyName("usage_in_usermode")]
        public ulong UsageInUsermode { get; set; } = default!;
    }
}
