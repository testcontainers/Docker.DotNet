namespace Docker.DotNet.Models
{
    public class PluginConfig // (plugin.Config)
    {
        [JsonPropertyName("Args")]
        public PluginArgs Args { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Documentation")]
        public string Documentation { get; set; }

        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; }

        [JsonPropertyName("Env")]
        public IList<PluginEnv> Env { get; set; }

        [JsonPropertyName("Interface")]
        public PluginInterface Interface { get; set; }

        [JsonPropertyName("IpcHost")]
        public bool IpcHost { get; set; }

        [JsonPropertyName("Linux")]
        public PluginLinuxConfig Linux { get; set; }

        [JsonPropertyName("Mounts")]
        public IList<PluginMount> Mounts { get; set; }

        [JsonPropertyName("Network")]
        public PluginNetworkConfig Network { get; set; }

        [JsonPropertyName("PidHost")]
        public bool PidHost { get; set; }

        [JsonPropertyName("PropagatedMount")]
        public string PropagatedMount { get; set; }

        [JsonPropertyName("User")]
        public PluginUser User { get; set; }

        [JsonPropertyName("WorkDir")]
        public string WorkDir { get; set; }

        [JsonPropertyName("rootfs")]
        public PluginRootFS Rootfs { get; set; }
    }
}
