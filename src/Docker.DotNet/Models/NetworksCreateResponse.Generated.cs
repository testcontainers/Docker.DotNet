#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CreateResponse NetworkCreateResponse
    /// 
    /// # OK response to NetworkCreate operation
    /// 
    /// swagger:model CreateResponse
    /// </summary>
    public class NetworksCreateResponse // (network.CreateResponse)
    {
        /// <summary>
        /// The ID of the created network.
        /// Example: b5c4fc71e8022147cd25de22b22173de4e3b170134117172eb595cb91b4e7e5d
        /// Required: true
        /// </summary>
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Warnings encountered when creating the container
        /// Required: true
        /// </summary>
        [JsonPropertyName("Warning")]
        public string Warning { get; set; } = default!;
    }
}
