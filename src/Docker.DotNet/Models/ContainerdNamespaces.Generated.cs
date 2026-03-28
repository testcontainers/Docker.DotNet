#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerdNamespaces // (system.ContainerdNamespaces)
    {
        [JsonPropertyName("Containers")]
        public string Containers { get; set; } = default!;

        [JsonPropertyName("Plugins")]
        public string Plugins { get; set; } = default!;
    }
}
