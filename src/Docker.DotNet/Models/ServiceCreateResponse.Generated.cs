#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ServiceCreateResponse contains the information returned to a client on the
    /// creation of a new service.
    /// 
    /// swagger:model ServiceCreateResponse
    /// </summary>
    public class ServiceCreateResponse // (swarm.ServiceCreateResponse)
    {
        /// <summary>
        /// The ID of the created service.
        /// Example: ak7w3gjqoa3kuz8xcpnyy0pvl
        /// </summary>
        [JsonPropertyName("ID")]
        public string? ID { get; set; }

        /// <summary>
        /// Optional warning message.
        /// 
        /// FIXME(thaJeztah): this should have &quot;omitempty&quot; in the generated type.
        /// 
        /// Example: [&quot;unable to pin image doesnotexist:latest to digest: image library/doesnotexist:latest not found&quot;]
        /// </summary>
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
