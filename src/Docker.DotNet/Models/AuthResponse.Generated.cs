#nullable enable
namespace Docker.DotNet.Models
{
    public class AuthResponse // (registry.AuthResponse)
    {
        [JsonPropertyName("IdentityToken")]
        public string IdentityToken { get; set; } = default!;

        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;
    }
}
