#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DeleteResponse delete response
    /// 
    /// swagger:model DeleteResponse
    /// </summary>
    public class ImageDeleteResponse // (image.DeleteResponse)
    {
        /// <summary>
        /// The image ID of an image that was deleted
        /// </summary>
        [JsonPropertyName("Deleted")]
        public string? Deleted { get; set; }

        /// <summary>
        /// The image ID of an image that was untagged
        /// </summary>
        [JsonPropertyName("Untagged")]
        public string? Untagged { get; set; }
    }
}
