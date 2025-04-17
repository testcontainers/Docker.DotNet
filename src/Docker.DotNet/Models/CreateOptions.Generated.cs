using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class CreateOptions // (network.CreateOptions)
    {
        [JsonPropertyName("Driver")]
        public string Driver { get; set; }

        [JsonPropertyName("Scope")]
        public string Scope { get; set; }

        [JsonPropertyName("EnableIPv4")]
        public bool? EnableIPv4 { get; set; }

        [JsonPropertyName("EnableIPv6")]
        public bool? EnableIPv6 { get; set; }

        [JsonPropertyName("IPAM")]
        public IPAM IPAM { get; set; }

        [JsonPropertyName("Internal")]
        public bool Internal { get; set; }

        [JsonPropertyName("Attachable")]
        public bool Attachable { get; set; }

        [JsonPropertyName("Ingress")]
        public bool Ingress { get; set; }

        [JsonPropertyName("ConfigOnly")]
        public bool ConfigOnly { get; set; }

        [JsonPropertyName("ConfigFrom")]
        public ConfigReference ConfigFrom { get; set; }

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; }
    }
}
