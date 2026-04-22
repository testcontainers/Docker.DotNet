#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Commit holds the Git-commit (SHA1) that a binary was built from, as reported
    /// in the version-string of external tools, such as containerd, or runC.
    /// </summary>
    public class Commit // (system.Commit)
    {
        /// <summary>
        /// ID is the actual commit ID or version of external tool.
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;
    }
}
