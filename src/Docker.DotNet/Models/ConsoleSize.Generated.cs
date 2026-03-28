#nullable enable
namespace Docker.DotNet.Models
{
    public class ConsoleSize // (client.ConsoleSize)
    {
        [JsonPropertyName("Height")]
        public ulong Height { get; set; } = default!;

        [JsonPropertyName("Width")]
        public ulong Width { get; set; } = default!;
    }
}
