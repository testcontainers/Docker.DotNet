#nullable enable
namespace Docker.DotNet.Models
{
    public class AuthConfig // (registry.AuthConfig)
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = default!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = default!;

        [JsonPropertyName("auth")]
        public string Auth { get; set; } = default!;

        [JsonPropertyName("serveraddress")]
        public string ServerAddress { get; set; } = default!;

        [JsonPropertyName("identitytoken")]
        public string IdentityToken { get; set; } = default!;

        [JsonPropertyName("registrytoken")]
        public string RegistryToken { get; set; } = default!;
    }
}
