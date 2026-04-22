#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PidsStats contains the stats of a container&apos;s pids
    /// </summary>
    public class PidsStats // (container.PidsStats)
    {
        /// <summary>
        /// Current is the number of pids in the cgroup
        /// </summary>
        [JsonPropertyName("current")]
        public ulong? Current { get; set; }

        /// <summary>
        /// Limit is the hard limit on the number of pids in the cgroup.
        /// A &quot;Limit&quot; of 0 means that there is no limit.
        /// </summary>
        [JsonPropertyName("limit")]
        public ulong? Limit { get; set; }
    }
}
