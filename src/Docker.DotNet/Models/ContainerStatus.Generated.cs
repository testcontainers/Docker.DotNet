#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerStatus // (swarm.ContainerStatus)
    {
        [JsonPropertyName("ContainerID")]
        public string ContainerID { get; set; } = default!;

        [JsonPropertyName("PID")]
        public long PID { get; set; } = default!;

        [JsonPropertyName("ExitCode")]
        public long ExitCode { get; set; } = default!;
    }
}
