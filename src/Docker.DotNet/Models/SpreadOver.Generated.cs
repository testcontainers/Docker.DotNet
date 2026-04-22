#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SpreadOver is a scheduling preference that instructs the scheduler to spread
    /// tasks evenly over groups of nodes identified by labels.
    /// </summary>
    public class SpreadOver // (swarm.SpreadOver)
    {
        /// <summary>
        /// label descriptor, such as engine.labels.az
        /// </summary>
        [JsonPropertyName("SpreadDescriptor")]
        public string SpreadDescriptor { get; set; } = default!;
    }
}
