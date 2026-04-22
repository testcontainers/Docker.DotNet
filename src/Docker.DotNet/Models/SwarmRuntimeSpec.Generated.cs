#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RuntimeSpec defines the base payload which clients can specify for creating
    /// a service with the plugin runtime.
    /// </summary>
    public class SwarmRuntimeSpec // (swarm.RuntimeSpec)
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("remote")]
        public string? Remote { get; set; }

        [JsonPropertyName("privileges")]
        public IList<RuntimePrivilege>? Privileges { get; set; }

        [JsonPropertyName("disabled")]
        public bool? Disabled { get; set; }

        [JsonPropertyName("env")]
        public IList<string>? Env { get; set; }
    }
}
