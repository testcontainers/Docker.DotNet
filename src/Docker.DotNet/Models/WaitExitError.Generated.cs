#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// WaitExitError container waiting error, if any
    /// 
    /// swagger:model WaitExitError
    /// </summary>
    public class WaitExitError // (container.WaitExitError)
    {
        /// <summary>
        /// Details of an error
        /// </summary>
        [JsonPropertyName("Message")]
        public string? Message { get; set; }
    }
}
