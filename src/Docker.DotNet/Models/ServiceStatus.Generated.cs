#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceStatus // (swarm.ServiceStatus)
    {
        [JsonPropertyName("RunningTasks")]
        public ulong RunningTasks { get; set; } = default!;

        [JsonPropertyName("DesiredTasks")]
        public ulong DesiredTasks { get; set; } = default!;

        [JsonPropertyName("CompletedTasks")]
        public ulong CompletedTasks { get; set; } = default!;
    }
}
