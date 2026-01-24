namespace Docker.DotNet.Models
{
    public class PluginRootFS // (plugin.RootFS)
    {
        [JsonPropertyName("diff_ids")]
        public IList<string> DiffIds { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
