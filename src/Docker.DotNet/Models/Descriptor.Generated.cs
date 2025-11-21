namespace Docker.DotNet.Models
{
    public class Descriptor // (v1.Descriptor)
    {
        [JsonPropertyName("mediaType")]
        public string MediaType { get; set; }

        [JsonPropertyName("digest")]
        public string Digest { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("urls")]
        public IList<string> URLs { get; set; }

        [JsonPropertyName("annotations")]
        public IDictionary<string, string> Annotations { get; set; }

        [JsonPropertyName("data")]
        [JsonConverter(typeof(Base64Converter))]
        public IList<byte> Data { get; set; }

        [JsonPropertyName("platform")]
        public Platform Platform { get; set; }

        [JsonPropertyName("artifactType")]
        public string ArtifactType { get; set; }
    }
}
