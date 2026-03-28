#nullable enable
namespace Docker.DotNet.Models
{
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
