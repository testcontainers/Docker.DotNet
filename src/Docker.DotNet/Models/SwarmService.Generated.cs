#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Service represents a service.
    /// </summary>
    public class SwarmService // (swarm.Service)
    {
        public SwarmService()
        {
        }

        public SwarmService(Meta Meta)
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

        [JsonPropertyName("Spec")]
        public ServiceSpec? Spec { get; set; }

        [JsonPropertyName("PreviousSpec")]
        public ServiceSpec? PreviousSpec { get; set; }

        [JsonPropertyName("Endpoint")]
        public Endpoint? Endpoint { get; set; }

        [JsonPropertyName("UpdateStatus")]
        public UpdateStatus? UpdateStatus { get; set; }

        /// <summary>
        /// ServiceStatus is an optional, extra field indicating the number of
        /// desired and running tasks. It is provided primarily as a shortcut to
        /// calculating these values client-side, which otherwise would require
        /// listing all tasks for a service, an operation that could be
        /// computation and network expensive.
        /// </summary>
        [JsonPropertyName("ServiceStatus")]
        public ServiceStatus? ServiceStatus { get; set; }

        /// <summary>
        /// JobStatus is the status of a Service which is in one of ReplicatedJob or
        /// GlobalJob modes. It is absent on Replicated and Global services.
        /// </summary>
        [JsonPropertyName("JobStatus")]
        public JobStatus? JobStatus { get; set; }
    }
}
