#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ExternalCA defines external CA to be used by the cluster.
    /// </summary>
    public class ExternalCA // (swarm.ExternalCA)
    {
        /// <summary>
        /// Protocol is the protocol used by this external CA.
        /// </summary>
        [JsonPropertyName("Protocol")]
        public string Protocol { get; set; } = default!;

        /// <summary>
        /// URL is the URL where the external CA can be reached.
        /// </summary>
        [JsonPropertyName("URL")]
        public string URL { get; set; } = default!;

        /// <summary>
        /// Options is a set of additional key/value pairs whose interpretation
        /// depends on the specified CA type.
        /// </summary>
        [JsonPropertyName("Options")]
        public IDictionary<string, string>? Options { get; set; }

        /// <summary>
        /// CACert specifies which root CA is used by this external CA.  This certificate must
        /// be in PEM format.
        /// </summary>
        [JsonPropertyName("CACert")]
        public string CACert { get; set; } = default!;
    }
}
