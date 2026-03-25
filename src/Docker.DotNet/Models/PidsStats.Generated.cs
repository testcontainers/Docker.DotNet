#nullable enable
namespace Docker.DotNet.Models
{
    public class PidsStats // (container.PidsStats)
    {
        [JsonPropertyName("current")]
        public ulong Current { get; set; } = default!;

        [JsonPropertyName("limit")]
        public ulong Limit { get; set; } = default!;
    }
}
