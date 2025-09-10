namespace Docker.DotNet.Models
{
    public class LogConfig // (container.LogConfig)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Config")]
        public IDictionary<string, string> Config { get; set; }
    }
}
