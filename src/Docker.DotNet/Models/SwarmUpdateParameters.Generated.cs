#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateParameters // (main.SwarmUpdateParameters)
    {
        [JsonPropertyName("Spec")]
        public Spec Spec { get; set; } = default!;

        [QueryStringParameter("version", true)]
        public long Version { get; set; } = default!;

        [QueryStringParameter<BoolQueryStringConverter>("rotateworkertoken", false)]
        public bool? RotateWorkerToken { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("rotatemanagertoken", false)]
        public bool? RotateManagerToken { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("rotatemanagerunlockkey", false)]
        public bool? RotateManagerUnlockKey { get; set; }
    }
}
