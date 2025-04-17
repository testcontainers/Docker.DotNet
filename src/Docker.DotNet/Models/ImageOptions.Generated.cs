using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class ImageOptions // (mount.ImageOptions)
    {
        [JsonPropertyName("Subpath")]
        public string Subpath { get; set; }
    }
}
