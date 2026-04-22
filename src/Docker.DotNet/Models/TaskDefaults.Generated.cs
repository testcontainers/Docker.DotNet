#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TaskDefaults parameterizes cluster-level task creation with default values.
    /// </summary>
    public class TaskDefaults // (swarm.TaskDefaults)
    {
        /// <summary>
        /// LogDriver selects the log driver to use for tasks created in the
        /// orchestrator if unspecified by a service.
        /// 
        /// Updating this value will only have an affect on new tasks. Old tasks
        /// will continue use their previously configured log driver until
        /// recreated.
        /// </summary>
        [JsonPropertyName("LogDriver")]
        public SwarmDriver? LogDriver { get; set; }
    }
}
