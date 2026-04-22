#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TopResponse ContainerTopResponse
    /// 
    /// Container &quot;top&quot; response.
    /// 
    /// swagger:model TopResponse
    /// </summary>
    public class ContainerProcessesResponse // (container.TopResponse)
    {
        /// <summary>
        /// Each process running in the container, where each process
        /// is an array of values corresponding to the titles.
        /// Example: {&quot;Processes&quot;:[[&quot;root&quot;,&quot;13642&quot;,&quot;882&quot;,&quot;0&quot;,&quot;17:03&quot;,&quot;pts/0&quot;,&quot;00:00:00&quot;,&quot;/bin/bash&quot;],[&quot;root&quot;,&quot;13735&quot;,&quot;13642&quot;,&quot;0&quot;,&quot;17:06&quot;,&quot;pts/0&quot;,&quot;00:00:00&quot;,&quot;sleep 10&quot;]]}
        /// </summary>
        [JsonPropertyName("Processes")]
        public IList<IList<string>> Processes { get; set; } = default!;

        /// <summary>
        /// The ps column titles
        /// Example: {&quot;Titles&quot;:[&quot;UID&quot;,&quot;PID&quot;,&quot;PPID&quot;,&quot;C&quot;,&quot;STIME&quot;,&quot;TTY&quot;,&quot;TIME&quot;,&quot;CMD&quot;]}
        /// </summary>
        [JsonPropertyName("Titles")]
        public IList<string> Titles { get; set; } = default!;
    }
}
