#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// AppArmorOpts defines the options for configuring AppArmor on a swarm-managed
    /// container.  Currently, custom AppArmor profiles are not supported.
    /// </summary>
    public class AppArmorOpts // (swarm.AppArmorOpts)
    {
        [JsonPropertyName("Mode")]
        public string? Mode { get; set; }
    }
}
