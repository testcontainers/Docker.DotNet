#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageHistoryResponse // (image.HistoryResponseItem)
    {
        [JsonPropertyName("Comment")]
        public string Comment { get; set; } = default!;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; } = default!;

        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;

        [JsonPropertyName("Tags")]
        public IList<string> Tags { get; set; } = default!;
    }
}
