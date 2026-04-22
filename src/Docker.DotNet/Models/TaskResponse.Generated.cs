#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Task represents a task.
    /// </summary>
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
        public Version? Version { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Spec")]
        public TaskSpec? Spec { get; set; }

        [JsonPropertyName("ServiceID")]
        public string? ServiceID { get; set; }

        [JsonPropertyName("Slot")]
        public long? Slot { get; set; }

        [JsonPropertyName("NodeID")]
        public string? NodeID { get; set; }

        [JsonPropertyName("Status")]
        public TaskStatus? Status { get; set; }

        [JsonPropertyName("DesiredState")]
        public TaskState? DesiredState { get; set; }

        [JsonPropertyName("NetworksAttachments")]
        public IList<NetworkAttachment>? NetworksAttachments { get; set; }

        [JsonPropertyName("GenericResources")]
        public IList<GenericResource>? GenericResources { get; set; }

        /// <summary>
        /// JobIteration is the JobIteration of the Service that this Task was
        /// spawned from, if the Service is a ReplicatedJob or GlobalJob. This is
        /// used to determine which Tasks belong to which run of the job. This field
        /// is absent if the Service mode is Replicated or Global.
        /// </summary>
        [JsonPropertyName("JobIteration")]
        public Version? JobIteration { get; set; }

        /// <summary>
        /// Volumes is the list of VolumeAttachments for this task. It specifies
        /// which particular volumes are to be used by this particular task, and
        /// fulfilling what mounts in the spec.
        /// </summary>
        [JsonPropertyName("Volumes")]
        public IList<VolumeAttachment> Volumes { get; set; } = default!;
    }
}
