#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Progress describes a progress message in a JSON stream.
    /// </summary>
    public class JSONProgress // (jsonstream.Progress)
    {
        /// <summary>
        /// Current is the current status and value of the progress made towards Total.
        /// </summary>
        [JsonPropertyName("current")]
        public long? Current { get; set; }

        /// <summary>
        /// Total is the end value describing when we made 100% progress for an operation.
        /// </summary>
        [JsonPropertyName("total")]
        public long? Total { get; set; }

        /// <summary>
        /// Start is the initial value for the operation.
        /// </summary>
        [JsonPropertyName("start")]
        public long? Start { get; set; }

        /// <summary>
        /// HideCounts. if true, hides the progress count indicator (xB/yB).
        /// </summary>
        [JsonPropertyName("hidecounts")]
        public bool? HideCounts { get; set; }

        /// <summary>
        /// Units is the unit to print for progress. It defaults to &quot;bytes&quot; if empty.
        /// </summary>
        [JsonPropertyName("units")]
        public string? Units { get; set; }
    }
}
