#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PortConfig represents the config of a port.
    /// </summary>
    public class PortConfig // (swarm.PortConfig)
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Protocol")]
        public string? Protocol { get; set; }

        /// <summary>
        /// TargetPort is the port inside the container
        /// </summary>
        [JsonPropertyName("TargetPort")]
        public uint? TargetPort { get; set; }

        /// <summary>
        /// PublishedPort is the port on the swarm hosts
        /// </summary>
        [JsonPropertyName("PublishedPort")]
        public uint? PublishedPort { get; set; }

        /// <summary>
        /// PublishMode is the mode in which port is published
        /// </summary>
        [JsonPropertyName("PublishMode")]
        public string? PublishMode { get; set; }
    }
}
