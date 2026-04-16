#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// InspectResponse is the response for the GET &quot;/containers/{name:.*}/json&quot;
    /// endpoint.
    /// </summary>
    public class ContainerInspectResponse // (container.InspectResponse)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        [JsonPropertyName("Path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("Args")]
        public IList<string> Args { get; set; } = default!;

        [JsonPropertyName("State")]
        public State? State { get; set; }

        [JsonPropertyName("Image")]
        public string Image { get; set; } = default!;

        [JsonPropertyName("ResolvConfPath")]
        public string ResolvConfPath { get; set; } = default!;

        [JsonPropertyName("HostnamePath")]
        public string HostnamePath { get; set; } = default!;

        [JsonPropertyName("HostsPath")]
        public string HostsPath { get; set; } = default!;

        [JsonPropertyName("LogPath")]
        public string LogPath { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("RestartCount")]
        public long RestartCount { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("Platform")]
        public string Platform { get; set; } = default!;

        [JsonPropertyName("MountLabel")]
        public string MountLabel { get; set; } = default!;

        [JsonPropertyName("ProcessLabel")]
        public string ProcessLabel { get; set; } = default!;

        [JsonPropertyName("AppArmorProfile")]
        public string AppArmorProfile { get; set; } = default!;

        [JsonPropertyName("ExecIDs")]
        public IList<string> ExecIDs { get; set; } = default!;

        [JsonPropertyName("HostConfig")]
        public HostConfig? HostConfig { get; set; }

        /// <summary>
        /// GraphDriver contains information about the container&apos;s graph driver.
        /// </summary>
        [JsonPropertyName("GraphDriver")]
        public DriverData? GraphDriver { get; set; }

        /// <summary>
        /// Storage contains information about the storage used for the container&apos;s filesystem.
        /// </summary>
        [JsonPropertyName("Storage")]
        public Storage? Storage { get; set; }

        [JsonPropertyName("SizeRw")]
        public long? SizeRw { get; set; }

        [JsonPropertyName("SizeRootFs")]
        public long? SizeRootFs { get; set; }

        [JsonPropertyName("Mounts")]
        public IList<MountPoint> Mounts { get; set; } = default!;

        [JsonPropertyName("Config")]
        public ContainerConfig? Config { get; set; }

        [JsonPropertyName("NetworkSettings")]
        public NetworkSettings? NetworkSettings { get; set; }

        /// <summary>
        /// ImageManifestDescriptor is the descriptor of a platform-specific manifest of the image used to create the container.
        /// </summary>
        [JsonPropertyName("ImageManifestDescriptor")]
        public Descriptor? ImageManifestDescriptor { get; set; }
    }
}
