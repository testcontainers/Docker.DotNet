#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Meta is a base object inherited by most of the other once.
    /// </summary>
    public class Meta // (swarm.Meta)
    {
        [JsonPropertyName("Version")]
        public Version? Version { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
