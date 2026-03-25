#nullable enable
namespace Docker.DotNet.Models
{
    public class JobStatus // (swarm.JobStatus)
    {
        [JsonPropertyName("JobIteration")]
        public Version JobIteration { get; set; } = default!;

        [JsonPropertyName("LastExecution")]
        public DateTime LastExecution { get; set; } = default!;
    }
}
