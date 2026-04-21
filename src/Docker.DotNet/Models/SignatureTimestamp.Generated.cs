#nullable enable
namespace Docker.DotNet.Models
{
    public class SignatureTimestamp // (image.SignatureTimestamp)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("URI")]
        public string URI { get; set; } = default!;

        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; } = default!;
    }
}
