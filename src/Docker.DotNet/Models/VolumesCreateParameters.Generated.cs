#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesCreateParameters // (main.VolumesCreateParameters)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("DriverOpts")]
        public IDictionary<string, string> DriverOpts { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;
    }
}
