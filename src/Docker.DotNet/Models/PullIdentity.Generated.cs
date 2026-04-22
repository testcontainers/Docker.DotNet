#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PullIdentity contains remote location information if image was created via pull.
    /// If image was pulled via mirror, this contains the original repository location.
    /// </summary>
    public class PullIdentity // (image.PullIdentity)
    {
        /// <summary>
        /// Repository is the remote repository location the image was pulled from.
        /// </summary>
        [JsonPropertyName("Repository")]
        public string? Repository { get; set; }
    }
}
