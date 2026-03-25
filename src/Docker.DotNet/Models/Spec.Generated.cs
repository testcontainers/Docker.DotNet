#nullable enable
namespace Docker.DotNet.Models
{
    public class Spec // (swarm.Spec)
    {
        public Spec()
        {
        }

        public Spec(Annotations Annotations)
        {
            if (Annotations != null)
            {
                this.Name = Annotations.Name;
                this.Labels = Annotations.Labels;
            }
        }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Orchestration")]
        public OrchestrationConfig Orchestration { get; set; } = default!;

        [JsonPropertyName("Raft")]
        public RaftConfig Raft { get; set; } = default!;

        [JsonPropertyName("Dispatcher")]
        public DispatcherConfig Dispatcher { get; set; } = default!;

        [JsonPropertyName("CAConfig")]
        public CAConfig CAConfig { get; set; } = default!;

        [JsonPropertyName("TaskDefaults")]
        public TaskDefaults TaskDefaults { get; set; } = default!;

        [JsonPropertyName("EncryptionConfig")]
        public EncryptionConfig EncryptionConfig { get; set; } = default!;
    }
}
