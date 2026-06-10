#nullable enable
namespace Docker.DotNet.Models
{
    public class Platform // (v1.Platform)
    {
        [JsonPropertyName("architecture")]
        public string Architecture { get; set; } = string.Empty;

        [JsonPropertyName("os")]
        public string OS { get; set; } = string.Empty;

        [JsonPropertyName("os.version")]
        public string? OSVersion { get; set; }

        [JsonPropertyName("os.features")]
        public IList<string>? OSFeatures { get; set; }

        [JsonPropertyName("variant")]
        public string? Variant { get; set; }
    }
}
