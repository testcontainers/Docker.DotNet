namespace Docker.DotNet.Models
{
    public class NetworkResponse // (network.Inspect)
    {
        public NetworkResponse()
        {
        }

        public NetworkResponse(Network Network)
        {
            if (Network != null)
            {
                this.Name = Network.Name;
                this.ID = Network.ID;
                this.Created = Network.Created;
                this.Scope = Network.Scope;
                this.Driver = Network.Driver;
                this.EnableIPv4 = Network.EnableIPv4;
                this.EnableIPv6 = Network.EnableIPv6;
                this.IPAM = Network.IPAM;
                this.Internal = Network.Internal;
                this.Attachable = Network.Attachable;
                this.Ingress = Network.Ingress;
                this.ConfigFrom = Network.ConfigFrom;
                this.ConfigOnly = Network.ConfigOnly;
                this.Options = Network.Options;
                this.Labels = Network.Labels;
                this.Peers = Network.Peers;
            }
        }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Id")]
        public string ID { get; set; }

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("Scope")]
        public string Scope { get; set; }

        [JsonPropertyName("Driver")]
        public string Driver { get; set; }

        [JsonPropertyName("EnableIPv4")]
        public bool EnableIPv4 { get; set; }

        [JsonPropertyName("EnableIPv6")]
        public bool EnableIPv6 { get; set; }

        [JsonPropertyName("IPAM")]
        public IPAM IPAM { get; set; }

        [JsonPropertyName("Internal")]
        public bool Internal { get; set; }

        [JsonPropertyName("Attachable")]
        public bool Attachable { get; set; }

        [JsonPropertyName("Ingress")]
        public bool Ingress { get; set; }

        [JsonPropertyName("ConfigFrom")]
        public ConfigReference ConfigFrom { get; set; }

        [JsonPropertyName("ConfigOnly")]
        public bool ConfigOnly { get; set; }

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; }

        [JsonPropertyName("Peers")]
        public IList<PeerInfo> Peers { get; set; }

        [JsonPropertyName("Containers")]
        public IDictionary<string, EndpointResource> Containers { get; set; }

        [JsonPropertyName("Services")]
        public IDictionary<string, ServiceInfo> Services { get; set; }

        [JsonPropertyName("Status")]
        public Status Status { get; set; }
    }
}
