#nullable enable
namespace Docker.DotNet.Models
{
    public class TmpfsOptions // (mount.TmpfsOptions)
    {
        [JsonPropertyName("SizeBytes")]
        public long SizeBytes { get; set; } = default!;

        [JsonPropertyName("Mode")]
        public uint Mode { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IList<IList<string>> Options { get; set; } = default!;
    }
}
