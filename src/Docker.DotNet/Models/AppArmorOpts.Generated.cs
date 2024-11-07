using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class AppArmorOpts // (swarm.AppArmorOpts)
    {
        [JsonPropertyName("Mode")]
        public string Mode { get; set; }
    }
}
