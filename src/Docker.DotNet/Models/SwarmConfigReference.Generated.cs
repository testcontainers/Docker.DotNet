#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ConfigReference is a reference to a config in swarm
    /// </summary>
    public class SwarmConfigReference // (swarm.ConfigReference)
    {
        [JsonPropertyName("File")]
        public ConfigReferenceFileTarget? File { get; set; }

        [JsonPropertyName("Runtime")]
        public ConfigReferenceRuntimeTarget? Runtime { get; set; }

        [JsonPropertyName("ConfigID")]
        public string ConfigID { get; set; } = default!;

        [JsonPropertyName("ConfigName")]
        public string ConfigName { get; set; } = default!;
    }
}
