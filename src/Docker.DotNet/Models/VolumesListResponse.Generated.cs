#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesListResponse // (main.VolumesListResponse)
    {
        [JsonPropertyName("Volumes")]
        public IList<VolumeResponse> Volumes { get; set; } = default!;

        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
