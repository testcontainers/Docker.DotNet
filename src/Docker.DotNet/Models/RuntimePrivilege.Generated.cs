namespace Docker.DotNet.Models
{
    public class RuntimePrivilege // (swarm.RuntimePrivilege)
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("value")]
        public IList<string> Value { get; set; }
    }
}
