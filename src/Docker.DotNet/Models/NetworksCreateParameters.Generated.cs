#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CreateRequest is the request message sent to the server for network create call.
    /// </summary>
    public class NetworksCreateParameters // (network.CreateRequest)
    {
        /// <summary>
        /// Name is the requested name of the network.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Driver is the driver-name used to create the network (e.g. `bridge`, `overlay`)
        /// </summary>
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        /// <summary>
        /// Scope describes the level at which the network exists (e.g. `swarm` for cluster-wide or `local` for machine level).
        /// </summary>
        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = default!;

        /// <summary>
        /// EnableIPv4 represents whether to enable IPv4.
        /// </summary>
        [JsonPropertyName("EnableIPv4")]
        public bool? EnableIPv4 { get; set; }

        /// <summary>
        /// EnableIPv6 represents whether to enable IPv6.
        /// </summary>
        [JsonPropertyName("EnableIPv6")]
        public bool? EnableIPv6 { get; set; }

        /// <summary>
        /// IPAM is the network&apos;s IP Address Management.
        /// </summary>
        [JsonPropertyName("IPAM")]
        public IPAM? IPAM { get; set; }

        /// <summary>
        /// Internal represents if the network is used internal only.
        /// </summary>
        [JsonPropertyName("Internal")]
        public bool Internal { get; set; } = default!;

        /// <summary>
        /// Attachable represents if the global scope is manually attachable by regular containers from workers in swarm mode.
        /// </summary>
        [JsonPropertyName("Attachable")]
        public bool Attachable { get; set; } = default!;

        /// <summary>
        /// Ingress indicates the network is providing the routing-mesh for the swarm cluster.
        /// </summary>
        [JsonPropertyName("Ingress")]
        public bool Ingress { get; set; } = default!;

        /// <summary>
        /// ConfigOnly creates a config-only network. Config-only networks are place-holder networks for network configurations to be used by other networks. ConfigOnly networks cannot be used directly to run containers or services.
        /// </summary>
        [JsonPropertyName("ConfigOnly")]
        public bool ConfigOnly { get; set; } = default!;

        /// <summary>
        /// ConfigFrom specifies the source which will provide the configuration for this network. The specified network must be a config-only network; see [CreateOptions.ConfigOnly].
        /// </summary>
        [JsonPropertyName("ConfigFrom")]
        public ConfigReference? ConfigFrom { get; set; }

        /// <summary>
        /// Options specifies the network-specific options to use for when creating the network.
        /// </summary>
        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        /// <summary>
        /// Labels holds metadata specific to the network being created.
        /// </summary>
        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;
    }
}
