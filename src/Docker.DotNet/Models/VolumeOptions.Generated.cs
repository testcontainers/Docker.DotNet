#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeOptions // (mount.VolumeOptions)
    {
        [JsonPropertyName("NoCopy")]
        public bool NoCopy { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Subpath")]
        public string Subpath { get; set; } = default!;

        [JsonPropertyName("DriverConfig")]
        public Driver? DriverConfig { get; set; }
    }
}
