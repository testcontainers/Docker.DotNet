#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ThrottleDevice is a structure that holds device:rate_per_second pair
    /// </summary>
    public class ThrottleDevice // (blkiodev.ThrottleDevice)
    {
        [JsonPropertyName("Path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("Rate")]
        public ulong Rate { get; set; } = default!;
    }
}
