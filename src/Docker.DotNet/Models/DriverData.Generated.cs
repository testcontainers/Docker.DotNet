#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DriverData Information about the storage driver used to store the container&apos;s and
    /// image&apos;s filesystem.
    /// 
    /// swagger:model DriverData
    /// </summary>
    public class DriverData // (storage.DriverData)
    {
        /// <summary>
        /// Low-level storage metadata, provided as key/value pairs.
        /// 
        /// This information is driver-specific, and depends on the storage-driver
        /// in use, and should be used for informational purposes only.
        /// 
        /// Example: {&quot;MergedDir&quot;:&quot;/var/lib/docker/overlay2/ef749362d13333e65fc95c572eb525abbe0052e16e086cb64bc3b98ae9aa6d74/merged&quot;,&quot;UpperDir&quot;:&quot;/var/lib/docker/overlay2/ef749362d13333e65fc95c572eb525abbe0052e16e086cb64bc3b98ae9aa6d74/diff&quot;,&quot;WorkDir&quot;:&quot;/var/lib/docker/overlay2/ef749362d13333e65fc95c572eb525abbe0052e16e086cb64bc3b98ae9aa6d74/work&quot;}
        /// Required: true
        /// </summary>
        [JsonPropertyName("Data")]
        public IDictionary<string, string> Data { get; set; } = default!;

        /// <summary>
        /// Name of the storage driver.
        /// Example: overlay2
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;
    }
}
