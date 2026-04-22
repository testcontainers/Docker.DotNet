#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAM represents IP Address Management
    /// </summary>
    public class IPAM // (network.IPAM)
    {
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        /// <summary>
        /// Per network IPAM driver options
        /// </summary>
        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        [JsonPropertyName("Config")]
        public IList<IPAMConfig> Config { get; set; } = default!;
    }
}
