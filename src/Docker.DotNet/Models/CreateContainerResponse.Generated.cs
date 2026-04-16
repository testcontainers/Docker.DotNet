#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CreateResponse ContainerCreateResponse
    /// 
    /// # OK response to ContainerCreate operation
    /// 
    /// swagger:model CreateResponse
    /// </summary>
    public class CreateContainerResponse // (container.CreateResponse)
    {
        /// <summary>
        /// The ID of the created container
        /// Example: ede54ee1afda366ab42f824e8a5ffd195155d853ceaec74a927f249ea270c743
        /// Required: true
        /// </summary>
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Warnings encountered when creating the container
        /// Example: []
        /// Required: true
        /// </summary>
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
