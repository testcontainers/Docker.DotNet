#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PlacementPreference provides a way to make the scheduler aware of factors
    /// such as topology.
    /// </summary>
    public class PlacementPreference // (swarm.PlacementPreference)
    {
        [JsonPropertyName("Spread")]
        public SpreadOver? Spread { get; set; }
    }
}
