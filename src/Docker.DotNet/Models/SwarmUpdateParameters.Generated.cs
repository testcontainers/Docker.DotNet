#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateParameters // (main.SwarmUpdateParameters)
    {
        [JsonPropertyName("Spec")]
        public Spec Spec { get; set; } = default!;

        [QueryStringParameter("version", true)]
        public long Version { get; set; } = default!;

        [QueryStringParameter("rotateworkertoken", false, typeof(QueryStringBoolConverter))]
        public bool? RotateWorkerToken { get; set; }

        [QueryStringParameter("rotatemanagertoken", false, typeof(QueryStringBoolConverter))]
        public bool? RotateManagerToken { get; set; }

        [QueryStringParameter("rotatemanagerunlockkey", false, typeof(QueryStringBoolConverter))]
        public bool? RotateManagerUnlockKey { get; set; }
    }
}
