#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeListResponse // (swarm.Node)
    {
        public NodeListResponse()
        {
        }

        public NodeListResponse(Meta Meta)
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
        public NodeUpdateParameters Spec { get; set; } = default!;

        [JsonPropertyName("Description")]
        public NodeDescription Description { get; set; } = default!;

        [JsonPropertyName("Status")]
        public NodeStatus Status { get; set; } = default!;

        [JsonPropertyName("ManagerStatus")]
        public ManagerStatus? ManagerStatus { get; set; }
    }
}
