#nullable enable
namespace Docker.DotNet.Models
{
    public class TaskResponse // (swarm.Task)
    {
        public TaskResponse()
        {
        }

        public TaskResponse(Meta Meta, Annotations Annotations)
        {
            if (Meta != null)
            {
                this.Version = Meta.Version;
                this.CreatedAt = Meta.CreatedAt;
                this.UpdatedAt = Meta.UpdatedAt;
            }

            if (Annotations != null)
            {
                this.Name = Annotations.Name;
                this.Labels = Annotations.Labels;
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

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Spec")]
        public TaskSpec Spec { get; set; } = default!;

        [JsonPropertyName("ServiceID")]
        public string ServiceID { get; set; } = default!;

        [JsonPropertyName("Slot")]
        public long Slot { get; set; } = default!;

        [JsonPropertyName("NodeID")]
        public string NodeID { get; set; } = default!;

        [JsonPropertyName("Status")]
        public TaskStatus Status { get; set; } = default!;

        [JsonPropertyName("DesiredState")]
        public TaskState DesiredState { get; set; } = default!;

        [JsonPropertyName("NetworksAttachments")]
        public IList<NetworkAttachment> NetworksAttachments { get; set; } = default!;

        [JsonPropertyName("GenericResources")]
        public IList<GenericResource> GenericResources { get; set; } = default!;

        [JsonPropertyName("JobIteration")]
        public Version? JobIteration { get; set; }

        [JsonPropertyName("Volumes")]
        public IList<VolumeAttachment> Volumes { get; set; } = default!;
    }
}
