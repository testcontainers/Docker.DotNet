using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class SummaryHostConfig // (container.Summary.HostConfig)
    {
        [JsonPropertyName("NetworkMode")]
        public string NetworkMode { get; set; }

        [JsonPropertyName("Annotations")]
        public IDictionary<string, string> Annotations { get; set; }
    }
}
