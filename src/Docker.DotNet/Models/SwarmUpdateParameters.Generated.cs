#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateParameters // (main.SwarmUpdateParameters)
    {
        [JsonPropertyName("Spec")]
        public Spec Spec { get; set; } = default!;

        [QueryStringParameter("version", true)]
        public long Version { get; set; } = default!;

        [QueryStringParameter<QueryStringBoolConverter>("rotateworkertoken", false)]
        public bool? RotateWorkerToken { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("rotatemanagertoken", false)]
        public bool? RotateManagerToken { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("rotatemanagerunlockkey", false)]
        public bool? RotateManagerUnlockKey { get; set; }
    }
}
