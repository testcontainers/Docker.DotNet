#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ServiceUpdateResponse service update response
    /// Example: {&quot;Warnings&quot;:[&quot;unable to pin image doesnotexist:latest to digest: image library/doesnotexist:latest not found&quot;]}
    /// 
    /// swagger:model ServiceUpdateResponse
    /// </summary>
    public class ServiceUpdateResponse // (swarm.ServiceUpdateResponse)
    {
        /// <summary>
        /// Optional warning messages
        /// </summary>
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
