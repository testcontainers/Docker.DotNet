using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class ImagePushParameters // (main.ImagePushParameters)
    {
        [QueryStringParameter("tag", false)]
        public string Tag { get; set; }

        [QueryStringParameter("platform", false)]
        public string Platform { get; set; }

        [JsonPropertyName("RegistryAuth")]
        public AuthConfig RegistryAuth { get; set; }
    }
}
