namespace Docker.DotNet.Models
{
    public class ComponentVersion // (system.ComponentVersion)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Version")]
        public string Version { get; set; }

        [JsonPropertyName("Details")]
        public IDictionary<string, string> Details { get; set; }
    }
}
