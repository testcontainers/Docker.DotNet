#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmInspectResponse // (swarm.Swarm)
    {
        public SwarmInspectResponse()
        {
        }

        public SwarmInspectResponse(ClusterInfo ClusterInfo)
        {
            if (ClusterInfo != null)
            {
                this.ID = ClusterInfo.ID;
                this.Version = ClusterInfo.Version;
                this.CreatedAt = ClusterInfo.CreatedAt;
                this.UpdatedAt = ClusterInfo.UpdatedAt;
                this.Spec = ClusterInfo.Spec;
                this.TLSInfo = ClusterInfo.TLSInfo;
                this.RootRotationInProgress = ClusterInfo.RootRotationInProgress;
                this.DefaultAddrPool = ClusterInfo.DefaultAddrPool;
                this.SubnetSize = ClusterInfo.SubnetSize;
                this.DataPathPort = ClusterInfo.DataPathPort;
            }
        }

        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Version")]
        public Version? Version { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("Spec")]
        public Spec Spec { get; set; } = default!;

        [JsonPropertyName("TLSInfo")]
        public TLSInfo TLSInfo { get; set; } = default!;

        [JsonPropertyName("RootRotationInProgress")]
        public bool RootRotationInProgress { get; set; } = default!;

        [JsonPropertyName("DefaultAddrPool")]
        public IList<string> DefaultAddrPool { get; set; } = default!;

        [JsonPropertyName("SubnetSize")]
        public uint SubnetSize { get; set; } = default!;

        [JsonPropertyName("DataPathPort")]
        public uint DataPathPort { get; set; } = default!;

        [JsonPropertyName("JoinTokens")]
        public JoinTokens JoinTokens { get; set; } = default!;
    }
}
