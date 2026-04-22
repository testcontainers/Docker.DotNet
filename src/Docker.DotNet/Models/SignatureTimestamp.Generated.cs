#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SignatureTimestamp contains information about a verified signed timestamp for an image signature.
    /// </summary>
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
