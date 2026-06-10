#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SecretReferenceFileTarget is a file target in a secret reference
    /// </summary>
    public class SecretReferenceFileTarget // (swarm.SecretReferenceFileTarget)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("UID")]
        public string UID { get; set; } = string.Empty;

        [JsonPropertyName("GID")]
        public string GID { get; set; } = string.Empty;

        [JsonPropertyName("Mode")]
        public uint Mode { get; set; } = default!;
    }
}
