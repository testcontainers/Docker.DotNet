#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SignatureIdentity contains the properties of verified signatures for the image.
    /// </summary>
    public class SignatureIdentity // (image.SignatureIdentity)
    {
        /// <summary>
        /// Name is a textual description summarizing the type of signature.
        /// </summary>
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Timestamps contains a list of verified signed timestamps for the signature.
        /// </summary>
        [JsonPropertyName("Timestamps")]
        public IList<SignatureTimestamp> Timestamps { get; set; } = default!;

        /// <summary>
        /// KnownSigner is an identifier for a special signer identity that is known to the implementation.
        /// </summary>
        [JsonPropertyName("KnownSigner")]
        public string? KnownSigner { get; set; }

        /// <summary>
        /// DockerReference is the Docker image reference associated with the signature.
        /// This is an optional field only present in older hashedrecord signatures.
        /// </summary>
        [JsonPropertyName("DockerReference")]
        public string? DockerReference { get; set; }

        /// <summary>
        /// Signer contains information about the signer certificate used to sign the image.
        /// </summary>
        [JsonPropertyName("Signer")]
        public SignerIdentity? Signer { get; set; }

        /// <summary>
        /// SignatureType is the type of signature format. E.g. &quot;bundle-v0.3&quot; or &quot;hashedrecord&quot;.
        /// </summary>
        [JsonPropertyName("SignatureType")]
        public string? SignatureType { get; set; }

        /// <summary>
        /// Error contains error information if signature verification failed.
        /// Other fields will be empty in this case.
        /// </summary>
        [JsonPropertyName("Error")]
        public string? Error { get; set; }

        /// <summary>
        /// Warnings contains any warnings that occurred during signature verification.
        /// For example, if there was no internet connectivity and cached trust roots were used.
        /// Warning does not indicate a failed verification but may point to configuration issues.
        /// </summary>
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
