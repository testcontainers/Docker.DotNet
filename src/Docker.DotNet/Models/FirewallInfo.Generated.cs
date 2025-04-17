using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class FirewallInfo // (system.FirewallInfo)
    {
        [JsonPropertyName("Driver")]
        public string Driver { get; set; }

        [JsonPropertyName("Info")]
        public IList<string[]> Info { get; set; }
    }
}
