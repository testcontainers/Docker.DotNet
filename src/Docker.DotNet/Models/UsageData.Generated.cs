#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// UsageData Usage details about the volume. This information is used by the
    /// `GET /system/df` endpoint, and omitted in other endpoints.
    /// 
    /// swagger:model UsageData
    /// </summary>
    public class UsageData // (volume.UsageData)
    {
        /// <summary>
        /// The number of containers referencing this volume. This field
        /// is set to `-1` if the reference-count is not available.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("RefCount")]
        public long RefCount { get; set; } = default!;

        /// <summary>
        /// Amount of disk space used by the volume (in bytes). This information
        /// is only available for volumes created with the `&quot;local&quot;` volume
        /// driver. For volumes created with other volume drivers, this field
        /// is set to `-1` (&quot;not available&quot;)
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;
    }
}
