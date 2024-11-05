using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class Platform // (v1.Platform)
    {
        [JsonPropertyName("architecture")]
        public string Architecture { get; set; }

        [JsonPropertyName("os")]
        public string OS { get; set; }

        [JsonPropertyName("os.version")]
        public string OSVersion { get; set; }

        [JsonPropertyName("os.features")]
        public IList<string> OSFeatures { get; set; }

        [JsonPropertyName("variant")]
        public string Variant { get; set; }
    }
}
