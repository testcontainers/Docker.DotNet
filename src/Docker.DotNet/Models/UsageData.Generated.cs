#nullable enable
namespace Docker.DotNet.Models
{
    public class UsageData // (volume.UsageData)
    {
        [JsonPropertyName("RefCount")]
        public long RefCount { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;
    }
}
