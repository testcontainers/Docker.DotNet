#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Task carries the information about one backend task
    /// 
    /// swagger:model Task
    /// </summary>
    public class NetworkTask // (network.Task)
    {
        /// <summary>
        /// name
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// endpoint ID
        /// </summary>
        [JsonPropertyName("EndpointID")]
        public string EndpointID { get; set; } = default!;

        /// <summary>
        /// endpoint IP
        /// </summary>
        [JsonPropertyName("EndpointIP")]
        public string EndpointIP { get; set; } = default!;

        /// <summary>
        /// info
        /// </summary>
        [JsonPropertyName("Info")]
        public IDictionary<string, string> Info { get; set; } = default!;
    }
}
