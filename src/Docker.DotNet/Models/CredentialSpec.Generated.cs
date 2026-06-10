#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CredentialSpec for managed service account (Windows only)
    /// </summary>
    public class CredentialSpec // (swarm.CredentialSpec)
    {
        [JsonPropertyName("Config")]
        public string Config { get; set; } = string.Empty;

        [JsonPropertyName("File")]
        public string File { get; set; } = string.Empty;

        [JsonPropertyName("Registry")]
        public string Registry { get; set; } = string.Empty;
    }
}
