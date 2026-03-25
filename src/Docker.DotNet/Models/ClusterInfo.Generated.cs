#nullable enable
namespace Docker.DotNet.Models
{
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
        public Version Version { get; set; } = default!;

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = default!;

        [JsonPropertyName("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = default!;

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
