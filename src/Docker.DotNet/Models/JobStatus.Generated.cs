#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// JobStatus is the status of a job-type service.
    /// </summary>
    public class JobStatus // (swarm.JobStatus)
    {
        /// <summary>
        /// JobIteration is a value increased each time a Job is executed,
        /// successfully or otherwise. &quot;Executed&quot;, in this case, means the job as a
        /// whole has been started, not that an individual Task has been launched. A
        /// job is &quot;Executed&quot; when its ServiceSpec is updated. JobIteration can be
        /// used to disambiguate Tasks belonging to different executions of a job.
        /// 
        /// Though JobIteration will increase with each subsequent execution, it may
        /// not necessarily increase by 1, and so JobIteration should not be used to
        /// keep track of the number of times a job has been executed.
        /// </summary>
        [JsonPropertyName("JobIteration")]
        public Version JobIteration { get; set; } = default!;

        /// <summary>
        /// LastExecution is the time that the job was last executed, as observed by
        /// Swarm manager.
        /// </summary>
        [JsonPropertyName("LastExecution")]
        public DateTime? LastExecution { get; set; }
    }
}
