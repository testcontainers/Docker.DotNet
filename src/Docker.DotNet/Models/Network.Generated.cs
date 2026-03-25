#nullable enable
namespace Docker.DotNet.Models
{
    public class Network // (network.Network)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("EnableIPv4")]
        public bool EnableIPv4 { get; set; } = default!;

        [JsonPropertyName("EnableIPv6")]
        public bool EnableIPv6 { get; set; } = default!;

        [JsonPropertyName("IPAM")]
        public IPAM IPAM { get; set; } = default!;

        [JsonPropertyName("Internal")]
        public bool Internal { get; set; } = default!;

        [JsonPropertyName("Attachable")]
        public bool Attachable { get; set; } = default!;

        [JsonPropertyName("Ingress")]
        public bool Ingress { get; set; } = default!;

        [JsonPropertyName("ConfigFrom")]
        public ConfigReference ConfigFrom { get; set; } = default!;

        [JsonPropertyName("ConfigOnly")]
        public bool ConfigOnly { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Peers")]
        public IList<PeerInfo> Peers { get; set; } = default!;
    }
}
