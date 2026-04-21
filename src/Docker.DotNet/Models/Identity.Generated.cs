#nullable enable
namespace Docker.DotNet.Models
{
    public class Identity // (image.Identity)
    {
        [JsonPropertyName("Signature")]
        public IList<SignatureIdentity> Signature { get; set; } = default!;

        [JsonPropertyName("Pull")]
        public IList<PullIdentity> Pull { get; set; } = default!;

        [JsonPropertyName("Build")]
        public IList<BuildIdentity> Build { get; set; } = default!;
    }
}
