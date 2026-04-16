#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkAttachmentSpec represents the runtime spec type for network
    /// attachment tasks
    /// </summary>
    public class NetworkAttachmentSpec // (swarm.NetworkAttachmentSpec)
    {
        [JsonPropertyName("ContainerID")]
        public string ContainerID { get; set; } = default!;
    }
}
