namespace Docker.DotNet.Models
{
    public class NetworksPruneResponse // (network.PruneReport)
    {
        [JsonPropertyName("NetworksDeleted")]
        public IList<string> NetworksDeleted { get; set; }
    }
}
