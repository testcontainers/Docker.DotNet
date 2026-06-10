#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SELinuxContext contains the SELinux labels of the container.
    /// </summary>
    public class SELinuxContext // (swarm.SELinuxContext)
    {
        [JsonPropertyName("Disable")]
        public bool Disable { get; set; } = default!;

        [JsonPropertyName("User")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("Role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("Level")]
        public string Level { get; set; } = string.Empty;
    }
}
