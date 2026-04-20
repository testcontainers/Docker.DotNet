#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SecretSpec represents a secret specification from a secret in swarm
    /// </summary>
    public class SwarmSecretSpec // (swarm.SecretSpec)
    {
        public SwarmSecretSpec()
        {
        }

        public SwarmSecretSpec(Annotations Annotations)
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
        /// Data is the data to store as a secret. It must be empty if a
        /// [Driver] is used, in which case the data is loaded from an external
        /// secret store. The maximum allowed size is 500KB, as defined in
        /// [MaxSecretSize].
        /// 
        /// This field is only used to create the secret, and is not returned
        /// by other endpoints.
        /// 
        /// [MaxSecretSize]: https://pkg.go.dev/github.com/moby/swarmkit/v2@v2.0.0/api/validation#MaxSecretSize
        /// </summary>
        [JsonPropertyName("Data")]
        public byte[]? Data { get; set; }

        /// <summary>
        /// Driver is the name of the secrets driver used to fetch the secret&apos;s
        /// value from an external secret store. If not set, the default built-in
        /// store is used.
        /// </summary>
        [JsonPropertyName("Driver")]
        public SwarmDriver? Driver { get; set; }

        /// <summary>
        /// Templating controls whether and how to evaluate the secret payload as
        /// a template. If it is not set, no templating is used.
        /// </summary>
        [JsonPropertyName("Templating")]
        public SwarmDriver? Templating { get; set; }
    }
}
