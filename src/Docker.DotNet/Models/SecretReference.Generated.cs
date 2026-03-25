#nullable enable
namespace Docker.DotNet.Models
{
    public class SecretReference // (swarm.SecretReference)
    {
        [JsonPropertyName("File")]
        public SecretReferenceFileTarget? File { get; set; }

        [JsonPropertyName("SecretID")]
        public string SecretID { get; set; } = default!;

        [JsonPropertyName("SecretName")]
        public string SecretName { get; set; } = default!;
    }
}
