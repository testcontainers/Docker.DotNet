#nullable enable
namespace Docker.DotNet.Models
{
    public class CapacityRange // (volume.CapacityRange)
    {
        [JsonPropertyName("RequiredBytes")]
        public long RequiredBytes { get; set; } = default!;

        [JsonPropertyName("LimitBytes")]
        public long LimitBytes { get; set; } = default!;
    }
}
