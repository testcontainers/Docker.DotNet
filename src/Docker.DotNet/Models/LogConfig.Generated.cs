#nullable enable
namespace Docker.DotNet.Models
{
    public class LogConfig // (container.LogConfig)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Config")]
        public IDictionary<string, string> Config { get; set; } = default!;
    }
}
