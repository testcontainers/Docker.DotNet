#nullable enable
namespace Docker.DotNet.Models
{
    public class SELinuxContext // (swarm.SELinuxContext)
    {
        [JsonPropertyName("Disable")]
        public bool Disable { get; set; } = default!;

        [JsonPropertyName("User")]
        public string User { get; set; } = default!;

        [JsonPropertyName("Role")]
        public string Role { get; set; } = default!;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Level")]
        public string Level { get; set; } = default!;
    }
}
