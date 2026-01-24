namespace Docker.DotNet.Models
{
    public class Storage // (storage.Storage)
    {
        [JsonPropertyName("RootFS")]
        public RootFSStorage RootFS { get; set; }
    }
}
