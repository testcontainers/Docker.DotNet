#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DeviceInfo represents a discoverable device from a device driver.
    /// </summary>
    public class DeviceInfo // (system.DeviceInfo)
    {
        /// <summary>
        /// Source indicates the origin device driver.
        /// </summary>
        [JsonPropertyName("Source")]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// ID is the unique identifier for the device.
        /// Example: CDI FQDN like &quot;vendor.com/gpu=0&quot;, or other driver-specific device ID
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; } = string.Empty;
    }
}
