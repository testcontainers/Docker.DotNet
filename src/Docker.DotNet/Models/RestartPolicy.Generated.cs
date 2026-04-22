#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RestartPolicy represents the restart policies of the container.
    /// </summary>
    public class RestartPolicy // (container.RestartPolicy)
    {
        [JsonPropertyName("Name")]
        public RestartPolicyKind Name { get; set; } = default!;

        [JsonPropertyName("MaximumRetryCount")]
        public long MaximumRetryCount { get; set; } = default!;
    }
}
