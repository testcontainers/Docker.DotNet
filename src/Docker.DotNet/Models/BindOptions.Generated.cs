#nullable enable
namespace Docker.DotNet.Models
{
    public class BindOptions // (mount.BindOptions)
    {
        [JsonPropertyName("Propagation")]
        public string Propagation { get; set; } = default!;

        [JsonPropertyName("NonRecursive")]
        public bool NonRecursive { get; set; } = default!;

        [JsonPropertyName("CreateMountpoint")]
        public bool CreateMountpoint { get; set; } = default!;

        [JsonPropertyName("ReadOnlyNonRecursive")]
        public bool ReadOnlyNonRecursive { get; set; } = default!;

        [JsonPropertyName("ReadOnlyForceRecursive")]
        public bool ReadOnlyForceRecursive { get; set; } = default!;
    }
}
