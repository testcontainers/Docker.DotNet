namespace Docker.DotNet.Models
{
    [JsonConverter(typeof(ConsoleSizeConverter))]
    public class ConsoleSize // (client.ConsoleSize)
    {
        [JsonPropertyName("Height")]
        public ulong Height { get; set; }

        [JsonPropertyName("Width")]
        public ulong Width { get; set; }
    }
}
