#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DeviceMapping represents the device mapping between the host and the container.
    /// </summary>
    public class DeviceMapping // (container.DeviceMapping)
    {
        [JsonPropertyName("PathOnHost")]
        public string PathOnHost { get; set; } = default!;

        [JsonPropertyName("PathInContainer")]
        public string PathInContainer { get; set; } = default!;

        [JsonPropertyName("CgroupPermissions")]
        public string CgroupPermissions { get; set; } = default!;
    }
}
