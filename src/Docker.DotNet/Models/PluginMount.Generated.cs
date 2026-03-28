#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginMount // (plugin.Mount)
    {
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("Destination")]
        public string Destination { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IList<string> Options { get; set; } = default!;

        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;

        [JsonPropertyName("Source")]
        public string? Source { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;
    }
}
