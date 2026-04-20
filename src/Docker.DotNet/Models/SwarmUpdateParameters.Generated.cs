#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateParameters // (main.SwarmUpdateParameters)
    {
        [JsonPropertyName("Spec")]
        public Spec Spec { get; set; } = default!;

        [QueryStringParameter("version", true)]
        public long Version { get; set; } = default!;

        [QueryStringBoolParameter("rotateworkertoken", false)]
        public bool? RotateWorkerToken { get; set; }

        [QueryStringBoolParameter("rotatemanagertoken", false)]
        public bool? RotateManagerToken { get; set; }

        [QueryStringBoolParameter("rotatemanagerunlockkey", false)]
        public bool? RotateManagerUnlockKey { get; set; }
    }
}
