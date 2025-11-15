namespace Docker.DotNet.Models
{
    public class RootFSStorage // (storage.RootFSStorage)
    {
        [JsonPropertyName("Snapshot")]
        public RootFSStorageSnapshot Snapshot { get; set; }
    }
}
