#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageBuildParameters // (main.ImageBuildParameters)
    {
        [QueryStringParameter<QueryStringEnumerableConverter>("t", false)]
        public IList<string>? Tags { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("q", false)]
        public bool? SuppressOutput { get; set; }

        [QueryStringParameter("remote", false)]
        public string? RemoteContext { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("nocache", false)]
        public bool? NoCache { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("rm", false)]
        public bool? Remove { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("forcerm", false)]
        public bool? ForceRemove { get; set; }

        [QueryStringParameter("pull", false)]
        public string? Pull { get; set; }

        [QueryStringParameter("cpusetcpus", false)]
        public string? CPUSetCPUs { get; set; }

        [QueryStringParameter("cpushares", false)]
        public long? CPUShares { get; set; }

        [QueryStringParameter("cpuquota", false)]
        public long? CPUQuota { get; set; }

        [QueryStringParameter("cpuperiod", false)]
        public long? CPUPeriod { get; set; }

        [QueryStringParameter("memory", false)]
        public long? Memory { get; set; }

        [QueryStringParameter("memswap", false)]
        public long? MemorySwap { get; set; }

        [QueryStringParameter("networkmode", false)]
        public string? NetworkMode { get; set; }

        [QueryStringParameter("shmsize", false)]
        public long? ShmSize { get; set; }

        [QueryStringParameter("dockerfile", false)]
        public string? Dockerfile { get; set; }

        [QueryStringParameter<QueryStringMapConverter>("buildargs", false)]
        public IDictionary<string, string>? BuildArgs { get; set; }

        [QueryStringParameter<QueryStringMapConverter>("labels", false)]
        public IDictionary<string, string>? Labels { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("squash", false)]
        public bool? Squash { get; set; }

        [QueryStringParameter<QueryStringEnumerableConverter>("cachefrom", false)]
        public IList<string>? CacheFrom { get; set; }

        [QueryStringParameter<QueryStringEnumerableConverter>("extrahosts", false)]
        public IList<string>? ExtraHosts { get; set; }

        [QueryStringParameter("target", false)]
        public string? Target { get; set; }

        [QueryStringParameter("platform", false)]
        public string? Platform { get; set; }

        [QueryStringParameter("outputs", false)]
        public string? Outputs { get; set; }

        [QueryStringParameter("version", false)]
        public string? Version { get; set; }

        [JsonPropertyName("AuthConfigs")]
        public IDictionary<string, AuthConfig> AuthConfigs { get; set; } = default!;
    }
}
