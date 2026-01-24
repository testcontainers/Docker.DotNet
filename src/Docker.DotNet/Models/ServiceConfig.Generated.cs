namespace Docker.DotNet.Models
{
    public class ServiceConfig // (registry.ServiceConfig)
    {
        [JsonPropertyName("InsecureRegistryCIDRs")]
        public IList<string> InsecureRegistryCIDRs { get; set; }

        [JsonPropertyName("IndexConfigs")]
        public IDictionary<string, IndexInfo> IndexConfigs { get; set; }

        [JsonPropertyName("Mirrors")]
        public IList<string> Mirrors { get; set; }
    }
}
