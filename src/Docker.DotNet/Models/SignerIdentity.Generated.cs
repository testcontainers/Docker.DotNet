#nullable enable
namespace Docker.DotNet.Models
{
    public class SignerIdentity // (image.SignerIdentity)
    {
        [JsonPropertyName("CertificateIssuer")]
        public string CertificateIssuer { get; set; } = default!;

        [JsonPropertyName("SubjectAlternativeName")]
        public string SubjectAlternativeName { get; set; } = default!;

        [JsonPropertyName("Issuer")]
        public string? Issuer { get; set; }

        [JsonPropertyName("BuildSignerURI")]
        public string? BuildSignerURI { get; set; }

        [JsonPropertyName("BuildSignerDigest")]
        public string? BuildSignerDigest { get; set; }

        [JsonPropertyName("RunnerEnvironment")]
        public string? RunnerEnvironment { get; set; }

        [JsonPropertyName("SourceRepositoryURI")]
        public string? SourceRepositoryURI { get; set; }

        [JsonPropertyName("SourceRepositoryDigest")]
        public string? SourceRepositoryDigest { get; set; }

        [JsonPropertyName("SourceRepositoryRef")]
        public string? SourceRepositoryRef { get; set; }

        [JsonPropertyName("SourceRepositoryIdentifier")]
        public string? SourceRepositoryIdentifier { get; set; }

        [JsonPropertyName("SourceRepositoryOwnerURI")]
        public string? SourceRepositoryOwnerURI { get; set; }

        [JsonPropertyName("SourceRepositoryOwnerIdentifier")]
        public string? SourceRepositoryOwnerIdentifier { get; set; }

        [JsonPropertyName("BuildConfigURI")]
        public string? BuildConfigURI { get; set; }

        [JsonPropertyName("BuildConfigDigest")]
        public string? BuildConfigDigest { get; set; }

        [JsonPropertyName("BuildTrigger")]
        public string? BuildTrigger { get; set; }

        [JsonPropertyName("RunInvocationURI")]
        public string? RunInvocationURI { get; set; }

        [JsonPropertyName("SourceRepositoryVisibilityAtSigning")]
        public string? SourceRepositoryVisibilityAtSigning { get; set; }
    }
}
