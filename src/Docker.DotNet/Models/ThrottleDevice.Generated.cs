#nullable enable
namespace Docker.DotNet.Models
{
    public class ThrottleDevice // (blkiodev.ThrottleDevice)
    {
        [JsonPropertyName("Path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("Rate")]
        public ulong Rate { get; set; } = default!;
    }
}
