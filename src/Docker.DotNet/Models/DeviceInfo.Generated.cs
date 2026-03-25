#nullable enable
namespace Docker.DotNet.Models
{
    public class DeviceInfo // (system.DeviceInfo)
    {
        [JsonPropertyName("Source")]
        public string Source { get; set; } = default!;

        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;
    }
}
