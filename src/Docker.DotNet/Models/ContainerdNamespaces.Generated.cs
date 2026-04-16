#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ContainerdNamespaces reflects the containerd namespaces used by the daemon.
    /// 
    /// These namespaces can be configured in the daemon configuration, and are
    /// considered to be used exclusively by the daemon,
    /// 
    /// As these namespaces are considered to be exclusively accessed
    /// by the daemon, it is not recommended to change these values,
    /// or to change them to a value that is used by other systems,
    /// such as cri-containerd.
    /// </summary>
    public class ContainerdNamespaces // (system.ContainerdNamespaces)
    {
        /// <summary>
        /// Containers holds the default containerd namespace used for
        /// containers managed by the daemon.
        /// 
        /// The default namespace for containers is &quot;moby&quot;, but will be
        /// suffixed with the `&lt;uid&gt;.&lt;gid&gt;` of the remapped `root` if
        /// user-namespaces are enabled and the containerd image-store
        /// is used.
        /// </summary>
        [JsonPropertyName("Containers")]
        public string Containers { get; set; } = default!;

        /// <summary>
        /// Plugins holds the default containerd namespace used for
        /// plugins managed by the daemon.
        /// 
        /// The default namespace for plugins is &quot;moby&quot;, but will be
        /// suffixed with the `&lt;uid&gt;.&lt;gid&gt;` of the remapped `root` if
        /// user-namespaces are enabled and the containerd image-store
        /// is used.
        /// </summary>
        [JsonPropertyName("Plugins")]
        public string Plugins { get; set; } = default!;
    }
}
