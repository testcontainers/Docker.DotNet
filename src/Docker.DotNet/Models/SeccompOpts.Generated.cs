#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SeccompOpts defines the options for configuring seccomp on a swarm-managed
    /// container.
    /// </summary>
    public class SeccompOpts // (swarm.SeccompOpts)
    {
        /// <summary>
        /// Mode is the SeccompMode used for the container.
        /// </summary>
        [JsonPropertyName("Mode")]
        public string? Mode { get; set; }

        /// <summary>
        /// Profile is the custom seccomp profile as a json object to be used with
        /// the container. Mode should be set to SeccompModeCustom when using a
        /// custom profile in this manner.
        /// </summary>
        [JsonPropertyName("Profile")]
        public IList<byte>? Profile { get; set; }
    }
}
