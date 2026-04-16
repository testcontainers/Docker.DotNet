#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// LogConfig represents the logging configuration of the container.
    /// </summary>
    public class LogConfig // (container.LogConfig)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Config")]
        public IDictionary<string, string> Config { get; set; } = default!;
    }
}
