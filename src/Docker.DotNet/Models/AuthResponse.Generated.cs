#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// AuthResponse An identity token was generated successfully.
    /// 
    /// swagger:model AuthResponse
    /// </summary>
    public class AuthResponse // (registry.AuthResponse)
    {
        /// <summary>
        /// An opaque token used to authenticate a user after a successful login
        /// Example: 9cbaf023786cd7...
        /// </summary>
        [JsonPropertyName("IdentityToken")]
        public string? IdentityToken { get; set; }

        /// <summary>
        /// The status of the authentication
        /// Example: Login Succeeded
        /// Required: true
        /// </summary>
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;
    }
}
