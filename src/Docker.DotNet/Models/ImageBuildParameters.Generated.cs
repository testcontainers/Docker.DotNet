#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageBuildParameters // (main.ImageBuildParameters)
    {
        [QueryStringParameter<EnumerableQueryStringConverter>("t", false)]
        public IList<string>? Tags { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("q", false)]
        public bool? SuppressOutput { get; set; }

        [QueryStringParameter("remote", false)]
        public string? RemoteContext { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("nocache", false)]
        public bool? NoCache { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("rm", false)]
        public bool? Remove { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("forcerm", false)]
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

        [QueryStringParameter<MapQueryStringConverter>("buildargs", false)]
        public IDictionary<string, string>? BuildArgs { get; set; }

        [QueryStringParameter<MapQueryStringConverter>("labels", false)]
        public IDictionary<string, string>? Labels { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("squash", false)]
        public bool? Squash { get; set; }

        [QueryStringParameter<EnumerableQueryStringConverter>("cachefrom", false)]
        public IList<string>? CacheFrom { get; set; }

        [QueryStringParameter<EnumerableQueryStringConverter>("extrahosts", false)]
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
