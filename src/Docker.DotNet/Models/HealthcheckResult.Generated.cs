#nullable enable
namespace Docker.DotNet.Models
{
    public class HealthcheckResult // (container.HealthcheckResult)
    {
        [JsonPropertyName("Start")]
        public DateTime Start { get; set; } = default!;

        [JsonPropertyName("End")]
        public DateTime End { get; set; } = default!;

        [JsonPropertyName("ExitCode")]
        public long ExitCode { get; set; } = default!;

        [JsonPropertyName("Output")]
        public string Output { get; set; } = default!;
    }
}
