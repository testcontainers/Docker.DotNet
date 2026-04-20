#nullable enable
namespace Docker.DotNet.Models
{
    public class SeccompOpts // (swarm.SeccompOpts)
    {
        [JsonPropertyName("Mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("Profile")]
        public byte[]? Profile { get; set; }
    }
}
