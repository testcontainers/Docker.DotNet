#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ClusterInfo represents info about the cluster for outputting in &quot;info&quot;
    /// it contains the same information as &quot;Swarm&quot;, but without the JoinTokens
    /// </summary>
    public class ClusterInfo // (swarm.ClusterInfo)
    {
        public ClusterInfo()
        {
        }

        public ClusterInfo(Meta Meta)
        {
            if (Meta != null)
            {
                this.Version = Meta.Version;
                this.CreatedAt = Meta.CreatedAt;
                this.UpdatedAt = Meta.UpdatedAt;
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
    }
}
