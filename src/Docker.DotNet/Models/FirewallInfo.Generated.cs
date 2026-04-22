#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// FirewallInfo describes the firewall backend.
    /// </summary>
    public class FirewallInfo // (system.FirewallInfo)
    {
        /// <summary>
        /// Driver is the name of the firewall backend driver.
        /// </summary>
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        /// <summary>
        /// Info is a list of label/value pairs, containing information related to the firewall.
        /// </summary>
        [JsonPropertyName("Info")]
        public IList<string[]>? Info { get; set; }
    }
}
