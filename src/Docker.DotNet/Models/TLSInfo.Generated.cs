namespace Docker.DotNet.Models
{
    public class TLSInfo // (swarm.TLSInfo)
    {
        [JsonPropertyName("TrustRoot")]
        public string TrustRoot { get; set; }

        [JsonPropertyName("CertIssuerSubject")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> CertIssuerSubject { get; set; }

        [JsonPropertyName("CertIssuerPublicKey")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> CertIssuerPublicKey { get; set; }
    }
}
