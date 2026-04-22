#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageProperties // (image.ImageProperties)
    {
        /// <summary>
        /// Platform is the OCI platform object describing the platform of the image.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Platform")]
        public Platform Platform { get; set; } = default!;

        [JsonPropertyName("Identity")]
        public Identity? Identity { get; set; }

        [JsonPropertyName("Size")]
        public ImagePropertiesSize Size { get; set; } = default!;

        /// <summary>
        /// Containers is an array containing the IDs of the containers that are
        /// using this image.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Containers")]
        public IList<string> Containers { get; set; } = default!;
    }
}
