#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// BuildIdentity contains build reference information if image was created via build.
    /// </summary>
    public class BuildIdentity // (image.BuildIdentity)
    {
        /// <summary>
        /// Ref is the identifier for the build request. This reference can be used to
        /// look up the build details in BuildKit history API.
        /// </summary>
        [JsonPropertyName("Ref")]
        public string? Ref { get; set; }

        /// <summary>
        /// CreatedAt is the time when the build ran.
        /// </summary>
        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }
    }
}
