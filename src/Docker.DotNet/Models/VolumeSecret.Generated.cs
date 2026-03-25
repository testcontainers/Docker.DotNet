#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeSecret // (volume.Secret)
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; } = default!;

        [JsonPropertyName("Secret")]
        public string Secret { get; set; } = default!;
    }
}
