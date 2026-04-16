#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ConfigReferenceFileTarget is a file target in a config reference
    /// </summary>
    public class ConfigReferenceFileTarget // (swarm.ConfigReferenceFileTarget)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("UID")]
        public string UID { get; set; } = default!;

        [JsonPropertyName("GID")]
        public string GID { get; set; } = default!;

        [JsonPropertyName("Mode")]
        public uint Mode { get; set; } = default!;
    }
}
