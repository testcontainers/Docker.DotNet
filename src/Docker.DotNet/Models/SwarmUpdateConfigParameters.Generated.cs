#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateConfigParameters // (main.SwarmUpdateConfigParameters)
    {
        [JsonPropertyName("Config")]
        public SwarmConfigSpec Config { get; set; } = default!;

        [QueryStringParameter("version", true)]
        public long Version { get; set; } = default!;
    }
}
