#nullable enable
namespace Docker.DotNet.Models
{
    public class CredentialSpec // (swarm.CredentialSpec)
    {
        [JsonPropertyName("Config")]
        public string Config { get; set; } = default!;

        [JsonPropertyName("File")]
        public string File { get; set; } = default!;

        [JsonPropertyName("Registry")]
        public string Registry { get; set; } = default!;
    }
}
