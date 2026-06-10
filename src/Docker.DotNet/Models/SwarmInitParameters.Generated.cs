#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// InitRequest is the request used to init a swarm.
    /// </summary>
    public class SwarmInitParameters // (swarm.InitRequest)
    {
        [JsonPropertyName("ListenAddr")]
        public string ListenAddr { get; set; } = string.Empty;

        [JsonPropertyName("AdvertiseAddr")]
        public string AdvertiseAddr { get; set; } = string.Empty;

        [JsonPropertyName("DataPathAddr")]
        public string DataPathAddr { get; set; } = string.Empty;

        [JsonPropertyName("DataPathPort")]
        public uint DataPathPort { get; set; } = default!;

        [JsonPropertyName("ForceNewCluster")]
        public bool ForceNewCluster { get; set; } = default!;

        [JsonPropertyName("Spec")]
        public Spec Spec { get; set; } = default!;

        [JsonPropertyName("AutoLockManagers")]
        public bool AutoLockManagers { get; set; } = default!;

        [JsonPropertyName("Availability")]
        public string Availability { get; set; } = string.Empty;

        [JsonPropertyName("DefaultAddrPool")]
        public IList<string> DefaultAddrPool { get; set; } = default!;

        [JsonPropertyName("SubnetSize")]
        public uint SubnetSize { get; set; } = default!;
    }
}
