#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginsInfo // (system.PluginsInfo)
    {
        [JsonPropertyName("Volume")]
        public IList<string> Volume { get; set; } = default!;

        [JsonPropertyName("Network")]
        public IList<string> Network { get; set; } = default!;

        [JsonPropertyName("Authorization")]
        public IList<string> Authorization { get; set; } = default!;

        [JsonPropertyName("Log")]
        public IList<string> Log { get; set; } = default!;
    }
}
