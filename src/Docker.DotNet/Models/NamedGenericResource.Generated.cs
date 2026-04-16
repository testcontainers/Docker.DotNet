#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NamedGenericResource represents a &quot;user defined&quot; resource which is defined
    /// as a string.
    /// &quot;Kind&quot; is used to describe the Kind of a resource (e.g: &quot;GPU&quot;, &quot;FPGA&quot;, &quot;SSD&quot;, ...)
    /// Value is used to identify the resource (GPU=&quot;UUID-1&quot;, FPGA=&quot;/dev/sdb5&quot;, ...)
    /// </summary>
    public class NamedGenericResource // (swarm.NamedGenericResource)
    {
        [JsonPropertyName("Kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("Value")]
        public string? Value { get; set; }
    }
}
