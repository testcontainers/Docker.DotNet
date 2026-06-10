#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// State stores container&apos;s running state
    /// it&apos;s part of ContainerJSONBase and returned by &quot;inspect&quot; command
    /// </summary>
    public class State // (container.State)
    {
        /// <summary>
        /// String representation of the container state. Can be one of &quot;created&quot;, &quot;running&quot;, &quot;paused&quot;, &quot;restarting&quot;, &quot;removing&quot;, &quot;exited&quot;, or &quot;dead&quot;
        /// </summary>
        [JsonPropertyName("Status")]
        public string Status { get; set; } = string.Empty;

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
        public string Error { get; set; } = string.Empty;

        [JsonPropertyName("StartedAt")]
        public string StartedAt { get; set; } = string.Empty;

        [JsonPropertyName("FinishedAt")]
        public string FinishedAt { get; set; } = string.Empty;

        [JsonPropertyName("Health")]
        public Health? Health { get; set; }
    }
}
