#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceUpdateParameters // (main.ServiceUpdateParameters)
    {
        [JsonPropertyName("Service")]
        public ServiceSpec Service { get; set; } = default!;

        [QueryStringParameter("version", true)]
        public long Version { get; set; } = default!;

        [QueryStringParameter("registryauthfrom", false)]
        public string? RegistryAuthFrom { get; set; }

        [QueryStringParameter("rollback", false)]
        public string? Rollback { get; set; }

        [JsonPropertyName("RegistryAuth")]
        public AuthConfig RegistryAuth { get; set; } = default!;
    }
}
