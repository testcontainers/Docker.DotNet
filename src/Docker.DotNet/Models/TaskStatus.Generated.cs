#nullable enable
namespace Docker.DotNet.Models
{
    public class TaskStatus // (swarm.TaskStatus)
    {
        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; } = default!;

        [JsonPropertyName("State")]
        public TaskState State { get; set; } = default!;

        [JsonPropertyName("Message")]
        public string Message { get; set; } = default!;

        [JsonPropertyName("Err")]
        public string Err { get; set; } = default!;

        [JsonPropertyName("ContainerStatus")]
        public ContainerStatus? ContainerStatus { get; set; }

        [JsonPropertyName("PortStatus")]
        public PortStatus PortStatus { get; set; } = default!;
    }
}
