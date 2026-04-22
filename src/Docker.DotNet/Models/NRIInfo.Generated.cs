#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NRIInfo describes the NRI configuration.
    /// </summary>
    public class NRIInfo // (system.NRIInfo)
    {
        [JsonPropertyName("Info")]
        public IList<string[]>? Info { get; set; }
    }
}
