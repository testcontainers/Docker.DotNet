#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PathStat is used to encode the header from
    /// GET &quot;/containers/{name:.*}/archive&quot;
    /// &quot;Name&quot; is the file or directory name.
    /// </summary>
    public class ContainerPathStatResponse // (container.PathStat)
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("size")]
        public long Size { get; set; } = default!;

        [JsonPropertyName("mode")]
        public uint Mode { get; set; } = default!;

        [JsonPropertyName("mtime")]
        public DateTime Mtime { get; set; } = default!;

        [JsonPropertyName("linkTarget")]
        public string LinkTarget { get; set; } = default!;
    }
}
