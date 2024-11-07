using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class ImageProperties // (image.ImageProperties)
    {
        [JsonPropertyName("Platform")]
        public Platform Platform { get; set; }

        [JsonPropertyName("Containers")]
        public IList<string> Containers { get; set; }
    }
}
