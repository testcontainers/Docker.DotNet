namespace Docker.DotNet.Models
{
    public class PluginConfigureParameters // (main.PluginConfigureParameters)
    {
        [JsonPropertyName("Args")]
        public IList<string> Args { get; set; }
    }
}
