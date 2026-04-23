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

        /// <summary>
        /// Identity holds information about the identity and origin of the image.
        /// For image list responses, this can duplicate Build/Pull fields across
        /// image manifests, because those parts of identity are image-level metadata.
        /// </summary>
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
