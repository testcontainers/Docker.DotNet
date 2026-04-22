#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ConfigReference The config-only network source to provide the configuration for
    /// this network.
    /// 
    /// swagger:model ConfigReference
    /// </summary>
    public class ConfigReference // (network.ConfigReference)
    {
        /// <summary>
        /// The name of the config-only network that provides the network&apos;s
        /// configuration. The specified network must be an existing config-only
        /// network. Only network names are allowed, not network IDs.
        /// 
        /// Example: config_only_network_01
        /// </summary>
        [JsonPropertyName("Network")]
        public string Network { get; set; } = default!;
    }
}
