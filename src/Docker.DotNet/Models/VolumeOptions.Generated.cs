#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// VolumeOptions represents the options for a mount of type volume.
    /// </summary>
    public class VolumeOptions // (mount.VolumeOptions)
    {
        [JsonPropertyName("NoCopy")]
        public bool? NoCopy { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string>? Labels { get; set; }

        [JsonPropertyName("Subpath")]
        public string? Subpath { get; set; }

        [JsonPropertyName("DriverConfig")]
        public Driver? DriverConfig { get; set; }
    }
}
