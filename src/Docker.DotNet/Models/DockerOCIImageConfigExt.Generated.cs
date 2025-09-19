namespace Docker.DotNet.Models
{
    public class DockerOCIImageConfigExt // (v1.DockerOCIImageConfigExt)
    {
        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig Healthcheck { get; set; }

        [JsonPropertyName("OnBuild")]
        public IList<string> OnBuild { get; set; }

        [JsonPropertyName("Shell")]
        public IList<string> Shell { get; set; }
    }
}
