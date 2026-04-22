#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RootFS root f s
    /// 
    /// swagger:model RootFS
    /// </summary>
    public class PluginRootFS // (plugin.RootFS)
    {
        /// <summary>
        /// diff ids
        /// Example: [&quot;sha256:675532206fbf3030b8458f88d6e26d4eb1577688a25efec97154c94e8b6b4887&quot;,&quot;sha256:e216a057b1cb1efc11f8a268f37ef62083e70b1b38323ba252e25ac88904a7e8&quot;]
        /// </summary>
        [JsonPropertyName("diff_ids")]
        public IList<string> DiffIds { get; set; } = default!;

        /// <summary>
        /// type
        /// Example: layers
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
