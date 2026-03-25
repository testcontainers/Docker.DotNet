#nullable enable
namespace Docker.DotNet.Models
{
    public class RestartPolicy // (container.RestartPolicy)
    {
        [JsonPropertyName("Name")]
        public RestartPolicyKind Name { get; set; } = default!;

        [JsonPropertyName("MaximumRetryCount")]
        public long MaximumRetryCount { get; set; } = default!;
    }
}
