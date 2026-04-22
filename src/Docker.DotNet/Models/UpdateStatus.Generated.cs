#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// UpdateStatus reports the status of a service update.
    /// </summary>
    public class UpdateStatus // (swarm.UpdateStatus)
    {
        [JsonPropertyName("State")]
        public string? State { get; set; }

        [JsonPropertyName("StartedAt")]
        public DateTime? StartedAt { get; set; }

        [JsonPropertyName("CompletedAt")]
        public DateTime? CompletedAt { get; set; }

        [JsonPropertyName("Message")]
        public string? Message { get; set; }
    }
}
