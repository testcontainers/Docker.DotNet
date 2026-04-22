#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SignerIdentity contains information about the signer certificate used to sign the image.
    /// This is [certificate.Summary] with deprecated fields removed and keys in Moby uppercase style.
    /// 
    /// [certificate.Summary]: https://pkg.go.dev/github.com/sigstore/sigstore-go/pkg/fulcio/certificate#Summary
    /// </summary>
    public class SignerIdentity // (image.SignerIdentity)
    {
        [JsonPropertyName("CertificateIssuer")]
        public string CertificateIssuer { get; set; } = default!;

        [JsonPropertyName("SubjectAlternativeName")]
        public string SubjectAlternativeName { get; set; } = default!;

        /// <summary>
        /// The OIDC issuer. Should match `iss` claim of ID token or, in the case of
        /// a federated login like Dex it should match the issuer URL of the
        /// upstream issuer. The issuer is not set the extensions are invalid and
        /// will fail to render.
        /// </summary>
        [JsonPropertyName("Issuer")]
        public string? Issuer { get; set; }

        /// <summary>
        /// Reference to specific build instructions that are responsible for signing.
        /// </summary>
        [JsonPropertyName("BuildSignerURI")]
        public string? BuildSignerURI { get; set; }

        /// <summary>
        /// Immutable reference to the specific version of the build instructions that is responsible for signing.
        /// </summary>
        [JsonPropertyName("BuildSignerDigest")]
        public string? BuildSignerDigest { get; set; }

        /// <summary>
        /// Specifies whether the build took place in platform-hosted cloud infrastructure or customer/self-hosted infrastructure.
        /// </summary>
        [JsonPropertyName("RunnerEnvironment")]
        public string? RunnerEnvironment { get; set; }

        /// <summary>
        /// Source repository URL that the build was based on.
        /// </summary>
        [JsonPropertyName("SourceRepositoryURI")]
        public string? SourceRepositoryURI { get; set; }

        /// <summary>
        /// Immutable reference to a specific version of the source code that the build was based upon.
        /// </summary>
        [JsonPropertyName("SourceRepositoryDigest")]
        public string? SourceRepositoryDigest { get; set; }

        /// <summary>
        /// Source Repository Ref that the build run was based upon.
        /// </summary>
        [JsonPropertyName("SourceRepositoryRef")]
        public string? SourceRepositoryRef { get; set; }

        /// <summary>
        /// Immutable identifier for the source repository the workflow was based upon.
        /// </summary>
        [JsonPropertyName("SourceRepositoryIdentifier")]
        public string? SourceRepositoryIdentifier { get; set; }

        /// <summary>
        /// Source repository owner URL of the owner of the source repository that the build was based on.
        /// </summary>
        [JsonPropertyName("SourceRepositoryOwnerURI")]
        public string? SourceRepositoryOwnerURI { get; set; }

        /// <summary>
        /// Immutable identifier for the owner of the source repository that the workflow was based upon.
        /// </summary>
        [JsonPropertyName("SourceRepositoryOwnerIdentifier")]
        public string? SourceRepositoryOwnerIdentifier { get; set; }

        /// <summary>
        /// Build Config URL to the top-level/initiating build instructions.
        /// </summary>
        [JsonPropertyName("BuildConfigURI")]
        public string? BuildConfigURI { get; set; }

        /// <summary>
        /// Immutable reference to the specific version of the top-level/initiating build instructions.
        /// </summary>
        [JsonPropertyName("BuildConfigDigest")]
        public string? BuildConfigDigest { get; set; }

        /// <summary>
        /// Event or action that initiated the build.
        /// </summary>
        [JsonPropertyName("BuildTrigger")]
        public string? BuildTrigger { get; set; }

        /// <summary>
        /// Run Invocation URL to uniquely identify the build execution.
        /// </summary>
        [JsonPropertyName("RunInvocationURI")]
        public string? RunInvocationURI { get; set; }

        /// <summary>
        /// Source repository visibility at the time of signing the certificate.
        /// </summary>
        [JsonPropertyName("SourceRepositoryVisibilityAtSigning")]
        public string? SourceRepositoryVisibilityAtSigning { get; set; }
    }
}
