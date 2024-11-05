using System;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class Metadata // (image.Metadata)
    {
        [JsonPropertyName("LastTagTime")]
        public DateTime LastTagTime { get; set; }
    }
}
