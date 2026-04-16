#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// OrchestrationConfig represents orchestration configuration.
    /// </summary>
    public class OrchestrationConfig // (swarm.OrchestrationConfig)
    {
        /// <summary>
        /// TaskHistoryRetentionLimit is the number of historic tasks to keep per instance or
        /// node. If negative, never remove completed or failed tasks.
        /// </summary>
        [JsonPropertyName("TaskHistoryRetentionLimit")]
        public long? TaskHistoryRetentionLimit { get; set; }
    }
}
