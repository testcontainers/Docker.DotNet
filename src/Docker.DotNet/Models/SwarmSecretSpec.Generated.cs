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
        public string Name { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; }

        [JsonPropertyName("Data")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> Data { get; set; }

        [JsonPropertyName("Driver")]
        public SwarmDriver Driver { get; set; }

        [JsonPropertyName("Templating")]
        public SwarmDriver Templating { get; set; }
    }
}
