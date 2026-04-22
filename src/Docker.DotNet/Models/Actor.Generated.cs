#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Actor describes something that generates events,
    /// like a container, or a network, or a volume.
    /// It has a defined name and a set of attributes.
    /// The container attributes are its labels, other actors
    /// can generate these attributes from other properties.
    /// </summary>
    public class Actor // (events.Actor)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Attributes")]
        public IDictionary<string, string> Attributes { get; set; } = default!;
    }
}
