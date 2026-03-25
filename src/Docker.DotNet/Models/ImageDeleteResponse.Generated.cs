#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageDeleteResponse // (image.DeleteResponse)
    {
        [JsonPropertyName("Deleted")]
        public string Deleted { get; set; } = default!;

        [JsonPropertyName("Untagged")]
        public string Untagged { get; set; } = default!;
    }
}
