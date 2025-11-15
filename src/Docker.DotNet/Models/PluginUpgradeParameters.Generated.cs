namespace Docker.DotNet.Models
{
    public class PluginUpgradeParameters // (main.PluginUpgradeParameters)
    {
        [QueryStringParameter("remote", true)]
        public string Remote { get; set; }

        [JsonPropertyName("RegistryAuth")]
        public AuthConfig RegistryAuth { get; set; }

        [JsonPropertyName("Privileges")]
        public IList<Privilege> Privileges { get; set; }
    }
}
