namespace Docker.DotNet.Models
{
    public class ImageProperties // (image.ImageProperties)
    {
        [JsonPropertyName("Platform")]
        public Platform Platform { get; set; }

        [JsonPropertyName("Size")]
        public ImagePropertiesSize Size { get; set; }

        [JsonPropertyName("Containers")]
        public IList<string> Containers { get; set; }
    }
}
