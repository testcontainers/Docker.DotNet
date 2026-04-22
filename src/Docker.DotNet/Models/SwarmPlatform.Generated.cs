#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Platform represents the platform (Arch/OS).
    /// </summary>
    public class SwarmPlatform // (swarm.Platform)
    {
        [JsonPropertyName("Architecture")]
        public string? Architecture { get; set; }

        [JsonPropertyName("OS")]
        public string? OS { get; set; }
    }
}
