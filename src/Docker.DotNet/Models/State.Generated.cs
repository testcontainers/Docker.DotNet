#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// State stores container&apos;s running state
    /// it&apos;s part of ContainerJSONBase and returned by &quot;inspect&quot; command
    /// </summary>
    public class State // (container.State)
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("Running")]
        public bool Running { get; set; } = default!;

        [JsonPropertyName("Paused")]
        public bool Paused { get; set; } = default!;

        [JsonPropertyName("Restarting")]
        public bool Restarting { get; set; } = default!;

        [JsonPropertyName("OOMKilled")]
        public bool OOMKilled { get; set; } = default!;

        [JsonPropertyName("Dead")]
        public bool Dead { get; set; } = default!;

        [JsonPropertyName("Pid")]
        public long Pid { get; set; } = default!;

        [JsonPropertyName("ExitCode")]
        public long ExitCode { get; set; } = default!;

        [JsonPropertyName("Error")]
        public string Error { get; set; } = default!;

        [JsonPropertyName("StartedAt")]
        public string StartedAt { get; set; } = default!;

        [JsonPropertyName("FinishedAt")]
        public string FinishedAt { get; set; } = default!;

        [JsonPropertyName("Health")]
        public Health? Health { get; set; }
    }
}
