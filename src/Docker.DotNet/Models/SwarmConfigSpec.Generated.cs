#nullable enable
namespace Docker.DotNet.Models
{
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
        public string Name { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Data")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> Data { get; set; } = default!;

        [JsonPropertyName("Templating")]
        public SwarmDriver? Templating { get; set; }
    }
}
