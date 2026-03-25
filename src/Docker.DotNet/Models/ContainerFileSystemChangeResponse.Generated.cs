#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerFileSystemChangeResponse // (container.FilesystemChange)
    {
        [JsonPropertyName("Kind")]
        public FileSystemChangeKind Kind { get; set; } = default!;

        [JsonPropertyName("Path")]
        public string Path { get; set; } = default!;
    }
}
