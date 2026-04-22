#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkAttachmentConfig represents the configuration of a network attachment.
    /// </summary>
    public class NetworkAttachmentConfig // (swarm.NetworkAttachmentConfig)
    {
        [JsonPropertyName("Target")]
        public string? Target { get; set; }

        [JsonPropertyName("Aliases")]
        public IList<string>? Aliases { get; set; }

        [JsonPropertyName("DriverOpts")]
        public IDictionary<string, string>? DriverOpts { get; set; }
    }
}
