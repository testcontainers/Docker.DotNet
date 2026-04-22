#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ReplicatedJob is the a type of Service which executes a defined Tasks
    /// in parallel until the specified number of Tasks have succeeded.
    /// </summary>
    public class ReplicatedJob // (swarm.ReplicatedJob)
    {
        /// <summary>
        /// MaxConcurrent indicates the maximum number of Tasks that should be
        /// executing simultaneously for this job at any given time. There may be
        /// fewer Tasks that MaxConcurrent executing simultaneously; for example, if
        /// there are fewer than MaxConcurrent tasks needed to reach
        /// TotalCompletions.
        /// 
        /// If this field is empty, it will default to a max concurrency of 1.
        /// </summary>
        [JsonPropertyName("MaxConcurrent")]
        public ulong? MaxConcurrent { get; set; }

        /// <summary>
        /// TotalCompletions is the total number of Tasks desired to run to
        /// completion.
        /// 
        /// If this field is empty, the value of MaxConcurrent will be used.
        /// </summary>
        [JsonPropertyName("TotalCompletions")]
        public ulong? TotalCompletions { get; set; }
    }
}
