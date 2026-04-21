#nullable enable
namespace Docker.DotNet.Models
{
    public class SignatureIdentity // (image.SignatureIdentity)
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Timestamps")]
        public IList<SignatureTimestamp> Timestamps { get; set; } = default!;

        [JsonPropertyName("KnownSigner")]
        public string? KnownSigner { get; set; }

        [JsonPropertyName("DockerReference")]
        public string? DockerReference { get; set; }

        [JsonPropertyName("Signer")]
        public SignerIdentity? Signer { get; set; }

        [JsonPropertyName("SignatureType")]
        public string? SignatureType { get; set; }

        [JsonPropertyName("Error")]
        public string? Error { get; set; }

        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
