#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// LinuxConfig linux config
    /// 
    /// swagger:model LinuxConfig
    /// </summary>
    public class PluginLinuxConfig // (plugin.LinuxConfig)
    {
        /// <summary>
        /// allow all devices
        /// Example: false
        /// Required: true
        /// </summary>
        [JsonPropertyName("AllowAllDevices")]
        public bool AllowAllDevices { get; set; } = default!;

        /// <summary>
        /// capabilities
        /// Example: [&quot;CAP_SYS_ADMIN&quot;,&quot;CAP_SYSLOG&quot;]
        /// Required: true
        /// </summary>
        [JsonPropertyName("Capabilities")]
        public IList<string> Capabilities { get; set; } = default!;

        /// <summary>
        /// devices
        /// Required: true
        /// </summary>
        [JsonPropertyName("Devices")]
        public IList<PluginDevice> Devices { get; set; } = default!;
    }
}
