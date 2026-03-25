#nullable enable
namespace Docker.DotNet.Models
{
    public class TLSInfo // (swarm.TLSInfo)
    {
        [JsonPropertyName("TrustRoot")]
        public string TrustRoot { get; set; } = default!;

        [JsonPropertyName("CertIssuerSubject")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> CertIssuerSubject { get; set; } = default!;

        [JsonPropertyName("CertIssuerPublicKey")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> CertIssuerPublicKey { get; set; } = default!;
    }
}
