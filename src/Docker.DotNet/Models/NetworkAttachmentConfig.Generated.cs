#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkAttachmentConfig // (swarm.NetworkAttachmentConfig)
    {
        [JsonPropertyName("Target")]
        public string Target { get; set; } = default!;

        [JsonPropertyName("Aliases")]
        public IList<string> Aliases { get; set; } = default!;

        [JsonPropertyName("DriverOpts")]
        public IDictionary<string, string> DriverOpts { get; set; } = default!;
    }
}
