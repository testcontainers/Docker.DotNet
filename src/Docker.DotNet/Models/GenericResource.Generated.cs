#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// GenericResource represents a &quot;user defined&quot; resource which can
    /// be either an integer (e.g: SSD=3) or a string (e.g: SSD=sda1)
    /// </summary>
    public class GenericResource // (swarm.GenericResource)
    {
        [JsonPropertyName("NamedResourceSpec")]
        public NamedGenericResource? NamedResourceSpec { get; set; }

        [JsonPropertyName("DiscreteResourceSpec")]
        public DiscreteGenericResource? DiscreteResourceSpec { get; set; }
    }
}
