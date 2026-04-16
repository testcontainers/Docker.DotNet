#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ResourceRequirements represents resources requirements.
    /// </summary>
    public class ResourceRequirements // (swarm.ResourceRequirements)
    {
        [JsonPropertyName("Limits")]
        public SwarmLimit? Limits { get; set; }

        [JsonPropertyName("Reservations")]
        public SwarmResources? Reservations { get; set; }

        /// <summary>
        /// Amount of swap in bytes - can only be used together with a memory limit
        /// -1 means unlimited
        /// a null pointer keeps the default behaviour of granting twice the memory
        /// amount in swap
        /// </summary>
        [JsonPropertyName("SwapBytes")]
        public long? SwapBytes { get; set; }

        /// <summary>
        /// Tune container memory swappiness (0 to 100) - if not specified, defaults
        /// to the container OS&apos;s default - generally 60, or the value predefined in
        /// the image; set to -1 to unset a previously set value
        /// </summary>
        [JsonPropertyName("MemorySwappiness")]
        public long? MemorySwappiness { get; set; }
    }
}
