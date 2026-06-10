#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DeviceMapping represents the device mapping between the host and the container.
    /// </summary>
    public class DeviceMapping // (container.DeviceMapping)
    {
        [JsonPropertyName("PathOnHost")]
        public string PathOnHost { get; set; } = string.Empty;

        [JsonPropertyName("PathInContainer")]
        public string PathInContainer { get; set; } = string.Empty;

        [JsonPropertyName("CgroupPermissions")]
        public string CgroupPermissions { get; set; } = string.Empty;
    }
}
