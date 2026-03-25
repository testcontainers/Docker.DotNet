#nullable enable
namespace Docker.DotNet.Models
{
    public class Secret // (swarm.Secret)
    {
        public Secret()
        {
        }

        public Secret(Meta Meta)
        {
            if (Meta != null)
            {
                this.Version = Meta.Version;
                this.CreatedAt = Meta.CreatedAt;
                this.UpdatedAt = Meta.UpdatedAt;
            }
        }

        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Version")]
        public Version Version { get; set; } = default!;

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = default!;

        [JsonPropertyName("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = default!;

        [JsonPropertyName("Spec")]
        public SwarmSecretSpec Spec { get; set; } = default!;
    }
}
