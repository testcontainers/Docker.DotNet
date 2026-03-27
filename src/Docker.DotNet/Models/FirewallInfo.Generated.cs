#nullable enable
namespace Docker.DotNet.Models
{
    public class FirewallInfo // (system.FirewallInfo)
    {
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("Info")]
        public IList<string[]>? Info { get; set; }
    }
}
