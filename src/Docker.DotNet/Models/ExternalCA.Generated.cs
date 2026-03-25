#nullable enable
namespace Docker.DotNet.Models
{
    public class ExternalCA // (swarm.ExternalCA)
    {
        [JsonPropertyName("Protocol")]
        public string Protocol { get; set; } = default!;

        [JsonPropertyName("URL")]
        public string URL { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        [JsonPropertyName("CACert")]
        public string CACert { get; set; } = default!;
    }
}
