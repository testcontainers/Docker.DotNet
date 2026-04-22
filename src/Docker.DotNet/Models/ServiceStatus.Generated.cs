#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ServiceStatus represents the number of running tasks in a service and the
    /// number of tasks desired to be running.
    /// </summary>
    public class ServiceStatus // (swarm.ServiceStatus)
    {
        /// <summary>
        /// RunningTasks is the number of tasks for the service actually in the
        /// Running state
        /// </summary>
        [JsonPropertyName("RunningTasks")]
        public ulong RunningTasks { get; set; } = default!;

        /// <summary>
        /// DesiredTasks is the number of tasks desired to be running by the
        /// service. For replicated services, this is the replica count. For global
        /// services, this is computed by taking the number of tasks with desired
        /// state of not-Shutdown.
        /// </summary>
        [JsonPropertyName("DesiredTasks")]
        public ulong DesiredTasks { get; set; } = default!;

        /// <summary>
        /// CompletedTasks is the number of tasks in the state Completed, if this
        /// service is in ReplicatedJob or GlobalJob mode. This field must be
        /// cross-referenced with the service type, because the default value of 0
        /// may mean that a service is not in a job mode, or it may mean that the
        /// job has yet to complete any tasks.
        /// </summary>
        [JsonPropertyName("CompletedTasks")]
        public ulong CompletedTasks { get; set; } = default!;
    }
}
