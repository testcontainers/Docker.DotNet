#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// MemoryStats aggregates all memory stats since container inception on Linux.
    /// Windows returns stats for commit and private working set only.
    /// </summary>
    public class MemoryStats // (container.MemoryStats)
    {
        /// <summary>
        /// current res_counter usage for memory
        /// </summary>
        [JsonPropertyName("usage")]
        public ulong? Usage { get; set; }

        /// <summary>
        /// maximum usage ever recorded.
        /// </summary>
        [JsonPropertyName("max_usage")]
        public ulong? MaxUsage { get; set; }

        /// <summary>
        /// TODO(vishh): Export these as stronger types.
        /// all the stats exported via memory.stat.
        /// </summary>
        [JsonPropertyName("stats")]
        public IDictionary<string, ulong>? Stats { get; set; }

        /// <summary>
        /// number of times memory usage hits limits.
        /// </summary>
        [JsonPropertyName("failcnt")]
        public ulong? Failcnt { get; set; }

        [JsonPropertyName("limit")]
        public ulong? Limit { get; set; }

        /// <summary>
        /// committed bytes
        /// </summary>
        [JsonPropertyName("commitbytes")]
        public ulong? Commit { get; set; }

        /// <summary>
        /// peak committed bytes
        /// </summary>
        [JsonPropertyName("commitpeakbytes")]
        public ulong? CommitPeak { get; set; }

        /// <summary>
        /// private working set
        /// </summary>
        [JsonPropertyName("privateworkingset")]
        public ulong? PrivateWorkingSet { get; set; }
    }
}
