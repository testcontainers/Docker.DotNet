#nullable enable
namespace Docker.DotNet.Models
{
    public class CPUUsage // (container.CPUUsage)
    {
        [JsonPropertyName("total_usage")]
        public ulong TotalUsage { get; set; } = default!;

        [JsonPropertyName("percpu_usage")]
        public IList<ulong> PercpuUsage { get; set; } = default!;

        [JsonPropertyName("usage_in_kernelmode")]
        public ulong UsageInKernelmode { get; set; } = default!;

        [JsonPropertyName("usage_in_usermode")]
        public ulong UsageInUsermode { get; set; } = default!;
    }
}
