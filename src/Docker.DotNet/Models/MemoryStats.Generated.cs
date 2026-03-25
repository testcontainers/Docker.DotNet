#nullable enable
namespace Docker.DotNet.Models
{
    public class MemoryStats // (container.MemoryStats)
    {
        [JsonPropertyName("usage")]
        public ulong Usage { get; set; } = default!;

        [JsonPropertyName("max_usage")]
        public ulong MaxUsage { get; set; } = default!;

        [JsonPropertyName("stats")]
        public IDictionary<string, ulong> Stats { get; set; } = default!;

        [JsonPropertyName("failcnt")]
        public ulong Failcnt { get; set; } = default!;

        [JsonPropertyName("limit")]
        public ulong Limit { get; set; } = default!;

        [JsonPropertyName("commitbytes")]
        public ulong Commit { get; set; } = default!;

        [JsonPropertyName("commitpeakbytes")]
        public ulong CommitPeak { get; set; } = default!;

        [JsonPropertyName("privateworkingset")]
        public ulong PrivateWorkingSet { get; set; } = default!;
    }
}
