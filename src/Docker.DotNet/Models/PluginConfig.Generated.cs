#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginConfig // (plugin.Config)
    {
        [JsonPropertyName("Args")]
        public PluginArgs Args { get; set; } = default!;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("Documentation")]
        public string Documentation { get; set; } = default!;

        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; } = default!;

        [JsonPropertyName("Env")]
        public IList<PluginEnv> Env { get; set; } = default!;

        [JsonPropertyName("Interface")]
        public PluginInterface Interface { get; set; } = default!;

        [JsonPropertyName("IpcHost")]
        public bool IpcHost { get; set; } = default!;

        [JsonPropertyName("Linux")]
        public PluginLinuxConfig Linux { get; set; } = default!;

        [JsonPropertyName("Mounts")]
        public IList<PluginMount> Mounts { get; set; } = default!;

        [JsonPropertyName("Network")]
        public PluginNetworkConfig Network { get; set; } = default!;

        [JsonPropertyName("PidHost")]
        public bool PidHost { get; set; } = default!;

        [JsonPropertyName("PropagatedMount")]
        public string PropagatedMount { get; set; } = default!;

        [JsonPropertyName("User")]
        public PluginUser User { get; set; } = default!;

        [JsonPropertyName("WorkDir")]
        public string WorkDir { get; set; } = default!;

        [JsonPropertyName("rootfs")]
        public PluginRootFS? Rootfs { get; set; }
    }
}
