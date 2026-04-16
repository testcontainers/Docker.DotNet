#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// AuthConfig contains authorization information for connecting to a Registry.
    /// </summary>
    public class AuthConfig // (registry.AuthConfig)
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("auth")]
        public string? Auth { get; set; }

        [JsonPropertyName("serveraddress")]
        public string? ServerAddress { get; set; }

        /// <summary>
        /// IdentityToken is used to authenticate the user and get
        /// an access token for the registry.
        /// </summary>
        [JsonPropertyName("identitytoken")]
        public string? IdentityToken { get; set; }

        /// <summary>
        /// RegistryToken is a bearer token to be sent to a registry
        /// </summary>
        [JsonPropertyName("registrytoken")]
        public string? RegistryToken { get; set; }
    }
}
