namespace Docker.DotNet.Models
{
    public class RuntimeSpec // (swarm.RuntimeSpec)
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("remote")]
        public string Remote { get; set; }

        [JsonPropertyName("privileges")]
        public IList<RuntimePrivilege> Privileges { get; set; }

        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        [JsonPropertyName("env")]
        public IList<string> Env { get; set; }
    }
}
