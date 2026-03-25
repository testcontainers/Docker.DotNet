#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceCreateParameters // (main.ServiceCreateParameters)
    {
        [JsonPropertyName("Service")]
        public ServiceSpec Service { get; set; } = default!;

        [JsonPropertyName("RegistryAuth")]
        public AuthConfig RegistryAuth { get; set; } = default!;
    }
}
