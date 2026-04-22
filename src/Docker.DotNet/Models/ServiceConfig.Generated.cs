#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ServiceConfig stores daemon registry services configuration.
    /// </summary>
    public class ServiceConfig // (registry.ServiceConfig)
    {
        [JsonPropertyName("InsecureRegistryCIDRs")]
        public IList<string> InsecureRegistryCIDRs { get; set; } = default!;

        [JsonPropertyName("IndexConfigs")]
        public IDictionary<string, IndexInfo> IndexConfigs { get; set; } = default!;

        [JsonPropertyName("Mirrors")]
        public IList<string> Mirrors { get; set; } = default!;
    }
}
