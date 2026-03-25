#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginInstallParameters // (main.PluginInstallParameters)
    {
        [QueryStringParameter("remote", true)]
        public string Remote { get; set; } = default!;

        [QueryStringParameter("name", false)]
        public string? Name { get; set; }

        [JsonPropertyName("RegistryAuth")]
        public AuthConfig RegistryAuth { get; set; } = default!;

        [JsonPropertyName("Privileges")]
        public IList<PluginPrivilege> Privileges { get; set; } = default!;
    }
}
