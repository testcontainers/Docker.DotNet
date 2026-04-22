#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Driver represents a volume driver.
    /// </summary>
    public class Driver // (mount.Driver)
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Options")]
        public IDictionary<string, string>? Options { get; set; }
    }
}
