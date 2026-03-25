#nullable enable
namespace Docker.DotNet.Models
{
    public class Platform // (v1.Platform)
    {
        [JsonPropertyName("architecture")]
        public string Architecture { get; set; } = default!;

        [JsonPropertyName("os")]
        public string OS { get; set; } = default!;

        [JsonPropertyName("os.version")]
        public string OSVersion { get; set; } = default!;

        [JsonPropertyName("os.features")]
        public IList<string> OSFeatures { get; set; } = default!;

        [JsonPropertyName("variant")]
        public string Variant { get; set; } = default!;
    }
}
