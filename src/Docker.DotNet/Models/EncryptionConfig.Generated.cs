#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EncryptionConfig controls at-rest encryption of data and keys.
    /// </summary>
    public class EncryptionConfig // (swarm.EncryptionConfig)
    {
        /// <summary>
        /// AutoLockManagers specifies whether or not managers TLS keys and raft data
        /// should be encrypted at rest in such a way that they must be unlocked
        /// before the manager node starts up again.
        /// </summary>
        [JsonPropertyName("AutoLockManagers")]
        public bool AutoLockManagers { get; set; } = default!;
    }
}
