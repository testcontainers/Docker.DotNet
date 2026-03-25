#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginUser // (plugin.User)
    {
        [JsonPropertyName("GID")]
        public uint GID { get; set; } = default!;

        [JsonPropertyName("UID")]
        public uint UID { get; set; } = default!;
    }
}
