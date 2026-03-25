#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginInterface // (plugin.Interface)
    {
        [JsonPropertyName("ProtocolScheme")]
        public string ProtocolScheme { get; set; } = default!;

        [JsonPropertyName("Socket")]
        public string Socket { get; set; } = default!;

        [JsonPropertyName("Types")]
        public IList<string> Types { get; set; } = default!;
    }
}
