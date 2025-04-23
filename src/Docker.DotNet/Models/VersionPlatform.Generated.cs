using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class VersionPlatform // (types.Version.Platform)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }
}
