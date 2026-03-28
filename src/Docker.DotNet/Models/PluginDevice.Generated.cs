#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginDevice // (plugin.Device)
    {
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Path")]
        public string? Path { get; set; }

        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;
    }
}
