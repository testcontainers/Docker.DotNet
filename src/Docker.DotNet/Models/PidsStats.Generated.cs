namespace Docker.DotNet.Models
{
    public class PidsStats // (container.PidsStats)
    {
        [JsonPropertyName("current")]
        public ulong Current { get; set; }

        [JsonPropertyName("limit")]
        public ulong Limit { get; set; }
    }
}
