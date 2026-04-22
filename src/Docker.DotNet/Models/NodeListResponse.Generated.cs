#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Node represents a node.
    /// </summary>
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
        public Version? Version { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Spec defines the desired state of the node as specified by the user.
        /// The system will honor this and will *never* modify it.
        /// </summary>
        [JsonPropertyName("Spec")]
        public NodeUpdateParameters? Spec { get; set; }

        /// <summary>
        /// Description encapsulates the properties of the Node as reported by the
        /// agent.
        /// </summary>
        [JsonPropertyName("Description")]
        public NodeDescription? Description { get; set; }

        /// <summary>
        /// Status provides the current status of the node, as seen by the manager.
        /// </summary>
        [JsonPropertyName("Status")]
        public NodeStatus? Status { get; set; }

        /// <summary>
        /// ManagerStatus provides the current status of the node&apos;s manager
        /// component, if the node is a manager.
        /// </summary>
        [JsonPropertyName("ManagerStatus")]
        public ManagerStatus? ManagerStatus { get; set; }
    }
}
