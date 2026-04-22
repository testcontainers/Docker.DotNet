#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DNSConfig specifies DNS related configurations in resolver configuration file (resolv.conf)
    /// Detailed documentation is available in:
    /// http://man7.org/linux/man-pages/man5/resolv.conf.5.html
    /// `nameserver`, `search`, `options` have been supported.
    /// TODO: `domain` is not supported yet.
    /// </summary>
    public class DNSConfig // (swarm.DNSConfig)
    {
        /// <summary>
        /// Nameservers specifies the IP addresses of the name servers
        /// </summary>
        [JsonPropertyName("Nameservers")]
        public IList<string>? Nameservers { get; set; }

        /// <summary>
        /// Search specifies the search list for host-name lookup
        /// </summary>
        [JsonPropertyName("Search")]
        public IList<string>? Search { get; set; }

        /// <summary>
        /// Options allows certain internal resolver variables to be modified
        /// </summary>
        [JsonPropertyName("Options")]
        public IList<string>? Options { get; set; }
    }
}
