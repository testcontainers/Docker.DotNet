#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Network network
    /// 
    /// swagger:model Network
    /// </summary>
    public class Network // (network.Network)
    {
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
    }
}
