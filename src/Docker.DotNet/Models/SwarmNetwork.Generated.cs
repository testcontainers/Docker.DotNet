#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmNetwork // (swarm.Network)
    {
        public SwarmNetwork()
        {
        }

        public SwarmNetwork(Meta Meta)
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
        public NetworkSpec Spec { get; set; } = default!;

        [JsonPropertyName("DriverState")]
        public SwarmDriver DriverState { get; set; } = default!;

        [JsonPropertyName("IPAMOptions")]
        public IPAMOptions? IPAMOptions { get; set; }
    }
}
