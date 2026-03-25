#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageProperties // (image.ImageProperties)
    {
        [JsonPropertyName("Platform")]
        public Platform Platform { get; set; } = default!;

        [JsonPropertyName("Size")]
        public ImagePropertiesSize Size { get; set; } = default!;

        [JsonPropertyName("Containers")]
        public IList<string> Containers { get; set; } = default!;
    }
}
