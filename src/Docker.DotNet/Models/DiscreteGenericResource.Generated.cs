#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DiscreteGenericResource represents a &quot;user-defined&quot; resource which is defined
    /// as an integer
    /// &quot;Kind&quot; is used to describe the Kind of a resource (e.g: &quot;GPU&quot;, &quot;FPGA&quot;, &quot;SSD&quot;, ...)
    /// Value is used to count the resource (SSD=5, HDD=3, ...)
    /// </summary>
    public class DiscreteGenericResource // (swarm.DiscreteGenericResource)
    {
        [JsonPropertyName("Kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("Value")]
        public long? Value { get; set; }
    }
}
