using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class SeccompOpts // (swarm.SeccompOpts)
    {
        [JsonPropertyName("Mode")]
        public string Mode { get; set; }

        [JsonPropertyName("Profile")]
        public IList<byte> Profile { get; set; }
    }
}
