#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DeviceRequest represents a request for devices from a device driver.
    /// Used by GPU device drivers.
    /// </summary>
    public class DeviceRequest // (container.DeviceRequest)
    {
        /// <summary>
        /// Name of device driver
        /// </summary>
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        /// <summary>
        /// Number of devices to request (-1 = All)
        /// </summary>
        [JsonPropertyName("Count")]
        public long Count { get; set; } = default!;

        /// <summary>
        /// List of device IDs as recognizable by the device driver
        /// </summary>
        [JsonPropertyName("DeviceIDs")]
        public IList<string> DeviceIDs { get; set; } = default!;

        /// <summary>
        /// An OR list of AND lists of device capabilities (e.g. &quot;gpu&quot;)
        /// </summary>
        [JsonPropertyName("Capabilities")]
        public IList<IList<string>> Capabilities { get; set; } = default!;

        /// <summary>
        /// Options to pass onto the device driver
        /// </summary>
        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;
    }
}
