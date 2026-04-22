#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// HealthcheckResult stores information about a single run of a healthcheck probe
    /// </summary>
    public class HealthcheckResult // (container.HealthcheckResult)
    {
        /// <summary>
        /// Start is the time this check started
        /// </summary>
        [JsonPropertyName("Start")]
        public DateTime Start { get; set; } = default!;

        /// <summary>
        /// End is the time this check ended
        /// </summary>
        [JsonPropertyName("End")]
        public DateTime End { get; set; } = default!;

        /// <summary>
        /// ExitCode meanings: 0=healthy, 1=unhealthy, 2=reserved (considered unhealthy), else=error running probe
        /// </summary>
        [JsonPropertyName("ExitCode")]
        public long ExitCode { get; set; } = default!;

        /// <summary>
        /// Output from last check
        /// </summary>
        [JsonPropertyName("Output")]
        public string Output { get; set; } = default!;
    }
}
