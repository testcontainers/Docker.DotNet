#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Inspect The body of the &quot;get network&quot; http response message.
    /// 
    /// swagger:model Inspect
    /// </summary>
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

        /// <summary>
        /// Name of the network.
        /// 
        /// Example: my_network
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// ID that uniquely identifies a network on a single machine.
        /// 
        /// Example: 7d86d31b1478e7cca9ebed7e73aa0fdeec46c5ca29497431d3007d2d9e15ed99
        /// </summary>
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Date and time at which the network was created in
        /// [RFC 3339](https://www.ietf.org/rfc/rfc3339.txt) format with nano-seconds.
        /// 
        /// Example: 2016-10-19T04:33:30.360899459Z
        /// </summary>
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        /// <summary>
        /// The level at which the network exists (e.g. `swarm` for cluster-wide
        /// or `local` for machine level)
        /// 
        /// Example: local
        /// </summary>
        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = default!;

        /// <summary>
        /// The name of the driver used to create the network (e.g. `bridge`,
        /// `overlay`).
        /// 
        /// Example: overlay
        /// </summary>
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        /// <summary>
        /// Whether the network was created with IPv4 enabled.
        /// 
        /// Example: true
        /// </summary>
        [JsonPropertyName("EnableIPv4")]
        public bool EnableIPv4 { get; set; } = default!;

        /// <summary>
        /// Whether the network was created with IPv6 enabled.
        /// 
        /// Example: false
        /// </summary>
        [JsonPropertyName("EnableIPv6")]
        public bool EnableIPv6 { get; set; } = default!;

        /// <summary>
        /// The network&apos;s IP Address Management.
        /// </summary>
        [JsonPropertyName("IPAM")]
        public IPAM IPAM { get; set; } = default!;

        /// <summary>
        /// Whether the network is created to only allow internal networking
        /// connectivity.
        /// 
        /// Example: false
        /// </summary>
        [JsonPropertyName("Internal")]
        public bool Internal { get; set; } = default!;

        /// <summary>
        /// Whether a global / swarm scope network is manually attachable by regular
        /// containers from workers in swarm mode.
        /// 
        /// Example: false
        /// </summary>
        [JsonPropertyName("Attachable")]
        public bool Attachable { get; set; } = default!;

        /// <summary>
        /// Whether the network is providing the routing-mesh for the swarm cluster.
        /// 
        /// Example: false
        /// </summary>
        [JsonPropertyName("Ingress")]
        public bool Ingress { get; set; } = default!;

        /// <summary>
        /// config from
        /// </summary>
        [JsonPropertyName("ConfigFrom")]
        public ConfigReference ConfigFrom { get; set; } = default!;

        /// <summary>
        /// Whether the network is a config-only network. Config-only networks are
        /// placeholder networks for network configurations to be used by other
        /// networks. Config-only networks cannot be used directly to run containers
        /// or services.
        /// </summary>
        [JsonPropertyName("ConfigOnly")]
        public bool ConfigOnly { get; set; } = default!;

        /// <summary>
        /// Network-specific options uses when creating the network.
        /// 
        /// Example: {&quot;com.docker.network.bridge.default_bridge&quot;:&quot;true&quot;,&quot;com.docker.network.bridge.enable_icc&quot;:&quot;true&quot;,&quot;com.docker.network.bridge.enable_ip_masquerade&quot;:&quot;true&quot;,&quot;com.docker.network.bridge.host_binding_ipv4&quot;:&quot;0.0.0.0&quot;,&quot;com.docker.network.bridge.name&quot;:&quot;docker0&quot;,&quot;com.docker.network.driver.mtu&quot;:&quot;1500&quot;}
        /// </summary>
        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        /// <summary>
        /// Metadata specific to the network being created.
        /// 
        /// Example: {&quot;com.example.some-label&quot;:&quot;some-value&quot;,&quot;com.example.some-other-label&quot;:&quot;some-other-value&quot;}
        /// </summary>
        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        /// <summary>
        /// List of peer nodes for an overlay network. This field is only present
        /// for overlay networks, and omitted for other network types.
        /// </summary>
        [JsonPropertyName("Peers")]
        public IList<PeerInfo>? Peers { get; set; }

        /// <summary>
        /// Contains endpoints attached to the network.
        /// 
        /// Example: {&quot;19a4d5d687db25203351ed79d478946f861258f018fe384f229f2efa4b23513c&quot;:{&quot;EndpointID&quot;:&quot;628cadb8bcb92de107b2a1e516cbffe463e321f548feb37697cce00ad694f21a&quot;,&quot;IPv4Address&quot;:&quot;172.19.0.2/16&quot;,&quot;IPv6Address&quot;:&quot;&quot;,&quot;MacAddress&quot;:&quot;02:42:ac:13:00:02&quot;,&quot;Name&quot;:&quot;test&quot;}}
        /// </summary>
        [JsonPropertyName("Containers")]
        public IDictionary<string, EndpointResource> Containers { get; set; } = default!;

        /// <summary>
        /// List of services using the network. This field is only present for
        /// swarm scope networks, and omitted for local scope networks.
        /// </summary>
        [JsonPropertyName("Services")]
        public IDictionary<string, ServiceInfo>? Services { get; set; }

        /// <summary>
        /// provides runtime information about the network such as the number of allocated IPs.
        /// </summary>
        [JsonPropertyName("Status")]
        public Status? Status { get; set; }
    }
}
