#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Metadata contains engine-local data about the image.
    /// </summary>
    public class Metadata // (image.Metadata)
    {
        /// <summary>
        /// LastTagTime is the date and time at which the image was last tagged.
        /// </summary>
        [JsonPropertyName("LastTagTime")]
        public DateTime? LastTagTime { get; set; }
    }
}
