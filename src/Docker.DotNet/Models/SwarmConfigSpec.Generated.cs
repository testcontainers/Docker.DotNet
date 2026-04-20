#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ConfigSpec represents a config specification from a config in swarm
    /// </summary>
    public class SwarmConfigSpec // (swarm.ConfigSpec)
    {
        public SwarmConfigSpec()
        {
        }

        public SwarmConfigSpec(Annotations Annotations)
        {
            if (Annotations != null)
            {
                this.Name = Annotations.Name;
                this.Labels = Annotations.Labels;
            }
        }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        /// <summary>
        /// Data is the data to store as a config.
        /// 
        /// The maximum allowed size is 1000KB, as defined in [MaxConfigSize].
        /// 
        /// [MaxConfigSize]: https://pkg.go.dev/github.com/moby/swarmkit/v2@v2.0.0-20250103191802-8c1959736554/manager/controlapi#MaxConfigSize
        /// </summary>
        [JsonPropertyName("Data")]
        public byte[]? Data { get; set; }

        /// <summary>
        /// Templating controls whether and how to evaluate the config payload as
        /// a template. If it is not set, no templating is used.
        /// </summary>
        [JsonPropertyName("Templating")]
        public SwarmDriver? Templating { get; set; }
    }
}
