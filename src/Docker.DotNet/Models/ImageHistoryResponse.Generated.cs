#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// HistoryResponseItem individual image layer information in response to ImageHistory operation
    /// 
    /// swagger:model HistoryResponseItem
    /// </summary>
    public class ImageHistoryResponse // (image.HistoryResponseItem)
    {
        /// <summary>
        /// comment
        /// Required: true
        /// </summary>
        [JsonPropertyName("Comment")]
        public string Comment { get; set; } = default!;

        /// <summary>
        /// created
        /// Required: true
        /// </summary>
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        /// <summary>
        /// created by
        /// Required: true
        /// </summary>
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; } = default!;

        /// <summary>
        /// Id
        /// Required: true
        /// </summary>
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// size
        /// Required: true
        /// </summary>
        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;

        /// <summary>
        /// tags
        /// Required: true
        /// </summary>
        [JsonPropertyName("Tags")]
        public IList<string> Tags { get; set; } = default!;
    }
}
