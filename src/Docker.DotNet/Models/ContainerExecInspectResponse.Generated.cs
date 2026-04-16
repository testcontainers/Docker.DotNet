#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ExecInspectResponse is the API response for the &quot;GET /exec/{id}/json&quot;
    /// endpoint and holds information about and exec.
    /// </summary>
    public class ContainerExecInspectResponse // (container.ExecInspectResponse)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Running")]
        public bool Running { get; set; } = default!;

        [JsonPropertyName("ExitCode")]
        public long? ExitCode { get; set; }

        [JsonPropertyName("ProcessConfig")]
        public ExecProcessConfig? ProcessConfig { get; set; }

        [JsonPropertyName("OpenStdin")]
        public bool OpenStdin { get; set; } = default!;

        [JsonPropertyName("OpenStderr")]
        public bool OpenStderr { get; set; } = default!;

        [JsonPropertyName("OpenStdout")]
        public bool OpenStdout { get; set; } = default!;

        [JsonPropertyName("CanRemove")]
        public bool CanRemove { get; set; } = default!;

        [JsonPropertyName("ContainerID")]
        public string ContainerID { get; set; } = default!;

        [JsonPropertyName("DetachKeys")]
        public string DetachKeys { get; set; } = default!;

        [JsonPropertyName("Pid")]
        public long Pid { get; set; } = default!;
    }
}
