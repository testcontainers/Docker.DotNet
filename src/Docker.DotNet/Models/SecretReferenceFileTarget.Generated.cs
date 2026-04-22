#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SecretReferenceFileTarget is a file target in a secret reference
    /// </summary>
    public class SecretReferenceFileTarget // (swarm.SecretReferenceFileTarget)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("UID")]
        public string UID { get; set; } = default!;

        [JsonPropertyName("GID")]
        public string GID { get; set; } = default!;

        [JsonPropertyName("Mode")]
        public uint Mode { get; set; } = default!;
    }
}
