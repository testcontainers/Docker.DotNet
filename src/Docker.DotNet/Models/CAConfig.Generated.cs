#nullable enable
namespace Docker.DotNet.Models
{
    public class CAConfig // (swarm.CAConfig)
    {
        [JsonPropertyName("NodeCertExpiry")]
        public long NodeCertExpiry { get; set; } = default!;

        [JsonPropertyName("ExternalCAs")]
        public IList<ExternalCA> ExternalCAs { get; set; } = default!;

        [JsonPropertyName("SigningCACert")]
        public string SigningCACert { get; set; } = default!;

        [JsonPropertyName("SigningCAKey")]
        public string SigningCAKey { get; set; } = default!;

        [JsonPropertyName("ForceRotate")]
        public ulong ForceRotate { get; set; } = default!;
    }
}
