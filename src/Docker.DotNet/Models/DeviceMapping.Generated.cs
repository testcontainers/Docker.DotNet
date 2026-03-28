#nullable enable
namespace Docker.DotNet.Models
{
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
