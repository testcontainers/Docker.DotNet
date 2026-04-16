#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// User user
    /// 
    /// swagger:model User
    /// </summary>
    public class PluginUser // (plugin.User)
    {
        /// <summary>
        /// g ID
        /// Example: 1000
        /// </summary>
        [JsonPropertyName("GID")]
        public uint? GID { get; set; }

        /// <summary>
        /// UID
        /// Example: 1000
        /// </summary>
        [JsonPropertyName("UID")]
        public uint? UID { get; set; }
    }
}
