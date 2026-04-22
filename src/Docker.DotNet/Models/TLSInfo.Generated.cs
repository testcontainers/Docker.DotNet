#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TLSInfo represents the TLS information about what CA certificate is trusted,
    /// and who the issuer for a TLS certificate is
    /// </summary>
    public class TLSInfo // (swarm.TLSInfo)
    {
        /// <summary>
        /// TrustRoot is the trusted CA root certificate in PEM format
        /// </summary>
        [JsonPropertyName("TrustRoot")]
        public string? TrustRoot { get; set; }

        /// <summary>
        /// CertIssuer is the raw subject bytes of the issuer
        /// </summary>
        [JsonPropertyName("CertIssuerSubject")]
        public byte[]? CertIssuerSubject { get; set; }

        /// <summary>
        /// CertIssuerPublicKey is the raw public key bytes of the issuer
        /// </summary>
        [JsonPropertyName("CertIssuerPublicKey")]
        public byte[]? CertIssuerPublicKey { get; set; }
    }
}
