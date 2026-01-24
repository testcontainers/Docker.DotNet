namespace Docker.DotNet.Models
{
    public class SwarmPlatform // (swarm.Platform)
    {
        [JsonPropertyName("Architecture")]
        public string Architecture { get; set; }

        [JsonPropertyName("OS")]
        public string OS { get; set; }
    }
}
