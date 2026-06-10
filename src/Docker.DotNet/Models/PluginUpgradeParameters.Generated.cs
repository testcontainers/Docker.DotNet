#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginUpgradeParameters // (main.PluginUpgradeParameters)
    {
        [QueryStringParameter("remote", true)]
        public string Remote { get; set; } = string.Empty;

        [JsonPropertyName("RegistryAuth")]
        public AuthConfig RegistryAuth { get; set; } = default!;

        [JsonPropertyName("Privileges")]
        public IList<PluginPrivilege> Privileges { get; set; } = default!;
    }
}
