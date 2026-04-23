#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Identity holds information about the identity and origin of the image.
    /// This is trusted information verified by the daemon and cannot be modified
    /// by tagging an image to a different name.
    /// </summary>
    public class Identity // (image.Identity)
    {
        /// <summary>
        /// Signature contains the properties of verified signatures for the image.
        /// </summary>
        [JsonPropertyName("Signature")]
        public IList<SignatureIdentity> Signature { get; set; } = default!;

        /// <summary>
        /// Pull contains remote location information if image was created via pull.
        /// If image was pulled via mirror, this contains the original repository location.
        /// After successful push this images also contains the pushed repository location.
        /// </summary>
        [JsonPropertyName("Pull")]
        public IList<PullIdentity> Pull { get; set; } = default!;

        /// <summary>
        /// Build contains build reference information if image was created via build.
        /// </summary>
        [JsonPropertyName("Build")]
        public IList<BuildIdentity> Build { get; set; } = default!;
    }
}
