#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// UpdateConfig represents the update configuration.
    /// </summary>
    public class SwarmUpdateConfig // (swarm.UpdateConfig)
    {
        /// <summary>
        /// Maximum number of tasks to be updated in one iteration.
        /// 0 means unlimited parallelism.
        /// </summary>
        [JsonPropertyName("Parallelism")]
        public ulong Parallelism { get; set; } = default!;

        /// <summary>
        /// Amount of time between updates.
        /// </summary>
        [JsonPropertyName("Delay")]
        public TimeSpan? Delay { get; set; }

        /// <summary>
        /// FailureAction is the action to take when an update failures.
        /// </summary>
        [JsonPropertyName("FailureAction")]
        public string? FailureAction { get; set; }

        /// <summary>
        /// Monitor indicates how long to monitor a task for failure after it is
        /// created. If the task fails by ending up in one of the states
        /// REJECTED, COMPLETED, or FAILED, within Monitor from its creation,
        /// this counts as a failure. If it fails after Monitor, it does not
        /// count as a failure. If Monitor is unspecified, a default value will
        /// be used.
        /// </summary>
        [JsonPropertyName("Monitor")]
        public TimeSpan? Monitor { get; set; }

        /// <summary>
        /// MaxFailureRatio is the fraction of tasks that may fail during
        /// an update before the failure action is invoked. Any task created by
        /// the current update which ends up in one of the states REJECTED,
        /// COMPLETED or FAILED within Monitor from its creation counts as a
        /// failure. The number of failures is divided by the number of tasks
        /// being updated, and if this fraction is greater than
        /// MaxFailureRatio, the failure action is invoked.
        /// 
        /// If the failure action is CONTINUE, there is no effect.
        /// If the failure action is PAUSE, no more tasks will be updated until
        /// another update is started.
        /// </summary>
        [JsonPropertyName("MaxFailureRatio")]
        public float MaxFailureRatio { get; set; } = default!;

        /// <summary>
        /// Order indicates the order of operations when rolling out an updated
        /// task. Either the old task is shut down before the new task is
        /// started, or the new task is started before the old task is shut down.
        /// </summary>
        [JsonPropertyName("Order")]
        public string Order { get; set; } = default!;
    }
}
