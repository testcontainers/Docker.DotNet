#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Version represents the internal object version.
    /// </summary>
    public class Version // (swarm.Version)
    {
        [JsonPropertyName("Index")]
        public ulong? Index { get; set; }
    }
}
