#nullable enable
namespace Docker.DotNet.Models
{
    public class ClusterVolume // (volume.ClusterVolume)
    {
        public ClusterVolume()
        {
        }

        public ClusterVolume(Meta Meta)
        {
            if (Meta != null)
            {
                this.Version = Meta.Version;
                this.CreatedAt = Meta.CreatedAt;
                this.UpdatedAt = Meta.UpdatedAt;
            }
        }

        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Version")]
        public Version Version { get; set; } = default!;

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = default!;

        [JsonPropertyName("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = default!;

        [JsonPropertyName("Spec")]
        public ClusterVolumeSpec Spec { get; set; } = default!;

        [JsonPropertyName("PublishStatus")]
        public IList<PublishStatus> PublishStatus { get; set; } = default!;

        [JsonPropertyName("Info")]
        public VolumeInfo? Info { get; set; }
    }
}
