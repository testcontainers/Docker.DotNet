namespace Docker.DotNet.Models
{
    public class ContainerExecInspectResponse // (container.ExecInspectResponse)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Running")]
        public bool Running { get; set; }

        [JsonPropertyName("ExitCode")]
        public long? ExitCode { get; set; }

        [JsonPropertyName("ProcessConfig")]
        public ExecProcessConfig ProcessConfig { get; set; }

        [JsonPropertyName("OpenStdin")]
        public bool OpenStdin { get; set; }

        [JsonPropertyName("OpenStderr")]
        public bool OpenStderr { get; set; }

        [JsonPropertyName("OpenStdout")]
        public bool OpenStdout { get; set; }

        [JsonPropertyName("CanRemove")]
        public bool CanRemove { get; set; }

        [JsonPropertyName("ContainerID")]
        public string ContainerID { get; set; }

        [JsonPropertyName("DetachKeys")]
        public IList<byte> DetachKeys { get; set; }

        [JsonPropertyName("Pid")]
        public long Pid { get; set; }
    }
}
