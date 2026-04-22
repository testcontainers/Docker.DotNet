#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Config The config of a plugin.
    /// 
    /// swagger:model Config
    /// </summary>
    public class PluginConfig // (plugin.Config)
    {
        /// <summary>
        /// args
        /// Required: true
        /// </summary>
        [JsonPropertyName("Args")]
        public PluginArgs Args { get; set; } = default!;

        /// <summary>
        /// description
        /// Example: A sample volume plugin for Docker
        /// Required: true
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        /// <summary>
        /// documentation
        /// Example: https://docs.docker.com/engine/extend/plugins/
        /// Required: true
        /// </summary>
        [JsonPropertyName("Documentation")]
        public string Documentation { get; set; } = default!;

        /// <summary>
        /// entrypoint
        /// Example: [&quot;/usr/bin/sample-volume-plugin&quot;,&quot;/data&quot;]
        /// Required: true
        /// </summary>
        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; } = default!;

        /// <summary>
        /// env
        /// Example: [{&quot;Description&quot;:&quot;If set, prints debug messages&quot;,&quot;Name&quot;:&quot;DEBUG&quot;,&quot;Settable&quot;:null,&quot;Value&quot;:&quot;0&quot;}]
        /// Required: true
        /// </summary>
        [JsonPropertyName("Env")]
        public IList<PluginEnv> Env { get; set; } = default!;

        /// <summary>
        /// interface
        /// Required: true
        /// </summary>
        [JsonPropertyName("Interface")]
        public PluginInterface Interface { get; set; } = default!;

        /// <summary>
        /// ipc host
        /// Example: false
        /// Required: true
        /// </summary>
        [JsonPropertyName("IpcHost")]
        public bool IpcHost { get; set; } = default!;

        /// <summary>
        /// linux
        /// Required: true
        /// </summary>
        [JsonPropertyName("Linux")]
        public PluginLinuxConfig Linux { get; set; } = default!;

        /// <summary>
        /// mounts
        /// Required: true
        /// </summary>
        [JsonPropertyName("Mounts")]
        public IList<PluginMount> Mounts { get; set; } = default!;

        /// <summary>
        /// network
        /// Required: true
        /// </summary>
        [JsonPropertyName("Network")]
        public PluginNetworkConfig Network { get; set; } = default!;

        /// <summary>
        /// pid host
        /// Example: false
        /// Required: true
        /// </summary>
        [JsonPropertyName("PidHost")]
        public bool PidHost { get; set; } = default!;

        /// <summary>
        /// propagated mount
        /// Example: /mnt/volumes
        /// Required: true
        /// </summary>
        [JsonPropertyName("PropagatedMount")]
        public string PropagatedMount { get; set; } = default!;

        /// <summary>
        /// user
        /// </summary>
        [JsonPropertyName("User")]
        public PluginUser? User { get; set; }

        /// <summary>
        /// work dir
        /// Example: /bin/
        /// Required: true
        /// </summary>
        [JsonPropertyName("WorkDir")]
        public string WorkDir { get; set; } = default!;

        /// <summary>
        /// rootfs
        /// </summary>
        [JsonPropertyName("rootfs")]
        public PluginRootFS? Rootfs { get; set; }
    }
}
