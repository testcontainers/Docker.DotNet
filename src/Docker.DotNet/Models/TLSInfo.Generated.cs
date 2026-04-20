#nullable enable
namespace Docker.DotNet.Models
{
    public class TLSInfo // (swarm.TLSInfo)
    {
        [JsonPropertyName("TrustRoot")]
        public string? TrustRoot { get; set; }

        [JsonPropertyName("CertIssuerSubject")]
        public byte[]? CertIssuerSubject { get; set; }

        [JsonPropertyName("CertIssuerPublicKey")]
        public byte[]? CertIssuerPublicKey { get; set; }
    }
}
