#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmRuntimeSpec // (swarm.RuntimeSpec)
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("remote")]
        public string Remote { get; set; } = default!;

        [JsonPropertyName("privileges")]
        public IList<RuntimePrivilege> Privileges { get; set; } = default!;

        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; } = default!;

        [JsonPropertyName("env")]
        public IList<string> Env { get; set; } = default!;
    }
}
