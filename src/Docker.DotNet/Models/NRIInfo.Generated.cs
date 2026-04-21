#nullable enable
namespace Docker.DotNet.Models
{
    public class NRIInfo // (system.NRIInfo)
    {
        [JsonPropertyName("Info")]
        public IList<string[]>? Info { get; set; }
    }
}
