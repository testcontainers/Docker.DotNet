#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ContainerdInfo holds information about the containerd instance used by the daemon.
    /// </summary>
    public class ContainerdInfo // (system.ContainerdInfo)
    {
        /// <summary>
        /// Address is the path to the containerd socket.
        /// </summary>
        [JsonPropertyName("Address")]
        public string? Address { get; set; }

        /// <summary>
        /// Namespaces is the containerd namespaces used by the daemon.
        /// </summary>
        [JsonPropertyName("Namespaces")]
        public ContainerdNamespaces Namespaces { get; set; } = default!;
    }
}
