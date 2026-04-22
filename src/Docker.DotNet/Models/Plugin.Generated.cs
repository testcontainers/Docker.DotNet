#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Plugin A plugin for the Engine API
    /// 
    /// swagger:model Plugin
    /// </summary>
    public class Plugin // (plugin.Plugin)
    {
        /// <summary>
        /// config
        /// Required: true
        /// </summary>
        [JsonPropertyName("Config")]
        public PluginConfig Config { get; set; } = default!;

        /// <summary>
        /// True if the plugin is running. False if the plugin is not running, only installed.
        /// Example: true
        /// Required: true
        /// </summary>
        [JsonPropertyName("Enabled")]
        public bool Enabled { get; set; } = default!;

        /// <summary>
        /// Id
        /// Example: 5724e2c8652da337ab2eedd19fc6fc0ec908e4bd907c7421bf6a8dfc70c4c078
        /// </summary>
        [JsonPropertyName("Id")]
        public string? ID { get; set; }

        /// <summary>
        /// name
        /// Example: tiborvass/sample-volume-plugin
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// plugin remote reference used to push/pull the plugin
        /// Example: localhost:5000/tiborvass/sample-volume-plugin:latest
        /// </summary>
        [JsonPropertyName("PluginReference")]
        public string? PluginReference { get; set; }

        /// <summary>
        /// settings
        /// Required: true
        /// </summary>
        [JsonPropertyName("Settings")]
        public PluginSettings Settings { get; set; } = default!;
    }
}
