using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class NetworkSettingsSummary // (container.NetworkSettingsSummary)
    {
        [JsonPropertyName("Networks")]
        public IDictionary<string, EndpointSettings> Networks { get; set; }
    }
}
