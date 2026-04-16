#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CAConfig represents CA configuration.
    /// </summary>
    public class CAConfig // (swarm.CAConfig)
    {
        /// <summary>
        /// NodeCertExpiry is the duration certificates should be issued for
        /// </summary>
        [JsonPropertyName("NodeCertExpiry")]
        public long? NodeCertExpiry { get; set; }

        /// <summary>
        /// ExternalCAs is a list of CAs to which a manager node will make
        /// certificate signing requests for node certificates.
        /// </summary>
        [JsonPropertyName("ExternalCAs")]
        public IList<ExternalCA>? ExternalCAs { get; set; }

        /// <summary>
        /// SigningCACert and SigningCAKey specify the desired signing root CA and
        /// root CA key for the swarm.  When inspecting the cluster, the key will
        /// be redacted.
        /// </summary>
        [JsonPropertyName("SigningCACert")]
        public string? SigningCACert { get; set; }

        [JsonPropertyName("SigningCAKey")]
        public string? SigningCAKey { get; set; }

        /// <summary>
        /// If this value changes, and there is no specified signing cert and key,
        /// then the swarm is forced to generate a new root certificate and key.
        /// </summary>
        [JsonPropertyName("ForceRotate")]
        public ulong? ForceRotate { get; set; }
    }
}
