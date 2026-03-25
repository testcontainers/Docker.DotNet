#nullable enable
namespace Docker.DotNet.Models
{
    public class PortSummary // (container.PortSummary)
    {
        [JsonPropertyName("IP")]
        public string IP { get; set; } = default!;

        [JsonPropertyName("PrivatePort")]
        public ushort PrivatePort { get; set; } = default!;

        [JsonPropertyName("PublicPort")]
        public ushort PublicPort { get; set; } = default!;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;
    }
}
