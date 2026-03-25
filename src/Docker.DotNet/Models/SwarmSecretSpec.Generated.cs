#nullable enable
namespace Docker.DotNet.Models
{
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
        public string Name { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Data")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> Data { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public SwarmDriver? Driver { get; set; }

        [JsonPropertyName("Templating")]
        public SwarmDriver? Templating { get; set; }
    }
}
