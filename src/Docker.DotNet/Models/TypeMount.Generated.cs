#nullable enable
namespace Docker.DotNet.Models
{
    public class TypeMount // (volume.TypeMount)
    {
        [JsonPropertyName("FsType")]
        public string FsType { get; set; } = default!;

        [JsonPropertyName("MountFlags")]
        public IList<string> MountFlags { get; set; } = default!;
    }
}
