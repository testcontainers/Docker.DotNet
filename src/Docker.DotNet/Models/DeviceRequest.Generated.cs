#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DeviceRequest represents a request for devices from a device driver.
    /// Used by GPU device drivers.
    /// </summary>
    public class DeviceRequest // (container.DeviceRequest)
    {
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("Count")]
        public long Count { get; set; } = default!;

        [JsonPropertyName("DeviceIDs")]
        public IList<string> DeviceIDs { get; set; } = default!;

        [JsonPropertyName("Capabilities")]
        public IList<IList<string>> Capabilities { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;
    }
}
