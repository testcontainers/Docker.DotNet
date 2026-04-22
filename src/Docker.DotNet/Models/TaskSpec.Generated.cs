#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TaskSpec represents the spec of a task.
    /// </summary>
    public class TaskSpec // (swarm.TaskSpec)
    {
        /// <summary>
        /// ContainerSpec, NetworkAttachmentSpec, and PluginSpec are mutually exclusive.
        /// PluginSpec is only used when the `Runtime` field is set to `plugin`
        /// NetworkAttachmentSpec is used if the `Runtime` field is set to
        /// `attachment`.
        /// </summary>
        [JsonPropertyName("ContainerSpec")]
        public ContainerSpec? ContainerSpec { get; set; }

        [JsonPropertyName("PluginSpec")]
        public SwarmRuntimeSpec? PluginSpec { get; set; }

        [JsonPropertyName("NetworkAttachmentSpec")]
        public NetworkAttachmentSpec? NetworkAttachmentSpec { get; set; }

        [JsonPropertyName("Resources")]
        public ResourceRequirements? Resources { get; set; }

        [JsonPropertyName("RestartPolicy")]
        public SwarmRestartPolicy? RestartPolicy { get; set; }

        [JsonPropertyName("Placement")]
        public Placement? Placement { get; set; }

        [JsonPropertyName("Networks")]
        public IList<NetworkAttachmentConfig>? Networks { get; set; }

        /// <summary>
        /// LogDriver specifies the LogDriver to use for tasks created from this
        /// spec. If not present, the one on cluster default on swarm.Spec will be
        /// used, finally falling back to the engine default if not specified.
        /// </summary>
        [JsonPropertyName("LogDriver")]
        public SwarmDriver? LogDriver { get; set; }

        /// <summary>
        /// ForceUpdate is a counter that triggers an update even if no relevant
        /// parameters have been changed.
        /// </summary>
        [JsonPropertyName("ForceUpdate")]
        public ulong ForceUpdate { get; set; } = default!;

        [JsonPropertyName("Runtime")]
        public string? Runtime { get; set; }
    }
}
