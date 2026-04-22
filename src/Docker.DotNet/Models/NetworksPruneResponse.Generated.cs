#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PruneReport contains the response for Engine API:
    /// POST &quot;/networks/prune&quot;
    /// </summary>
    public class NetworksPruneResponse // (network.PruneReport)
    {
        [JsonPropertyName("NetworksDeleted")]
        public IList<string> NetworksDeleted { get; set; } = default!;
    }
}
