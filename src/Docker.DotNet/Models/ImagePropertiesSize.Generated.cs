namespace Docker.DotNet.Models
{
    public class ImagePropertiesSize // (image.ImageProperties.Size)
    {
        [JsonPropertyName("Unpacked")]
        public long Unpacked { get; set; }
    }
}
