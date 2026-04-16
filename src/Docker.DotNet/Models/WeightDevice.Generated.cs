#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// WeightDevice is a structure that holds device:weight pair
    /// </summary>
    public class WeightDevice // (blkiodev.WeightDevice)
    {
        [JsonPropertyName("Path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("Weight")]
        public ushort Weight { get; set; } = default!;
    }
}
