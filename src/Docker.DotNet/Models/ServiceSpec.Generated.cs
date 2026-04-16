#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ServiceSpec represents the spec of a service.
    /// </summary>
    public class ServiceSpec // (swarm.ServiceSpec)
    {
        public ServiceSpec()
        {
        }

        public ServiceSpec(Annotations Annotations)
        {
            if (Annotations != null)
            {
                this.Name = Annotations.Name;
                this.Labels = Annotations.Labels;
            }
        }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        /// <summary>
        /// TaskTemplate defines how the service should construct new tasks when
        /// orchestrating this service.
        /// </summary>
        [JsonPropertyName("TaskTemplate")]
        public TaskSpec? TaskTemplate { get; set; }

        [JsonPropertyName("Mode")]
        public ServiceMode? Mode { get; set; }

        [JsonPropertyName("UpdateConfig")]
        public SwarmUpdateConfig? UpdateConfig { get; set; }

        [JsonPropertyName("RollbackConfig")]
        public SwarmUpdateConfig? RollbackConfig { get; set; }

        [JsonPropertyName("EndpointSpec")]
        public EndpointSpec? EndpointSpec { get; set; }
    }
}
