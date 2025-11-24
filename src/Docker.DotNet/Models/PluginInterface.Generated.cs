namespace Docker.DotNet.Models
{
    public class PluginInterface // (plugin.Interface)
    {
        [JsonPropertyName("ProtocolScheme")]
        public string ProtocolScheme { get; set; }

        [JsonPropertyName("Socket")]
        public string Socket { get; set; }

        [JsonPropertyName("Types")]
        public IList<string> Types { get; set; }
    }
}
