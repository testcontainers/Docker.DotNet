#nullable enable
namespace Docker.DotNet.Models
{
    public class IPAM // (network.IPAM)
    {
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        [JsonPropertyName("Config")]
        public IList<IPAMConfig> Config { get; set; } = default!;
    }
}
