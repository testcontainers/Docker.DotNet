#nullable enable
namespace Docker.DotNet.Models
{
    public class AttestationProperties // (image.AttestationProperties)
    {
        /// <summary>
        /// For is the digest of the image manifest that this attestation is for.
        /// </summary>
        [JsonPropertyName("For")]
        public string For { get; set; } = default!;
    }
}
