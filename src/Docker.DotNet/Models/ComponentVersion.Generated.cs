#nullable enable
namespace Docker.DotNet.Models
{
    public class ComponentVersion // (system.ComponentVersion)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Version")]
        public string Version { get; set; } = default!;

        [JsonPropertyName("Details")]
        public IDictionary<string, string> Details { get; set; } = default!;
    }
}
