using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class NetworksCreateParameters // (network.CreateRequest)
    {
        public NetworksCreateParameters()
        {
        }

        public NetworksCreateParameters(CreateOptions CreateOptions)
        {
            if (CreateOptions != null)
            {
                this.Driver = CreateOptions.Driver;
                this.Scope = CreateOptions.Scope;
                this.EnableIPv4 = CreateOptions.EnableIPv4;
                this.EnableIPv6 = CreateOptions.EnableIPv6;
                this.IPAM = CreateOptions.IPAM;
                this.Internal = CreateOptions.Internal;
                this.Attachable = CreateOptions.Attachable;
                this.Ingress = CreateOptions.Ingress;
                this.ConfigOnly = CreateOptions.ConfigOnly;
                this.ConfigFrom = CreateOptions.ConfigFrom;
                this.Options = CreateOptions.Options;
                this.Labels = CreateOptions.Labels;
            }
        }

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

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("CheckDuplicate")]
        public bool? CheckDuplicate { get; set; }
    }
}
