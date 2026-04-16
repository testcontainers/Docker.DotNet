#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PortSummary Describes a port-mapping between the container and the host.
    /// 
    /// Example: {&quot;PrivatePort&quot;:8080,&quot;PublicPort&quot;:80,&quot;Type&quot;:&quot;tcp&quot;}
    /// 
    /// swagger:model PortSummary
    /// </summary>
    public class PortSummary // (container.PortSummary)
    {
        /// <summary>
        /// Host IP address that the container&apos;s port is mapped to
        /// </summary>
        [JsonPropertyName("IP")]
        public string? IP { get; set; }

        /// <summary>
        /// Port on the container
        /// Required: true
        /// </summary>
        [JsonPropertyName("PrivatePort")]
        public ushort PrivatePort { get; set; } = default!;

        /// <summary>
        /// Port exposed on the host
        /// </summary>
        [JsonPropertyName("PublicPort")]
        public ushort? PublicPort { get; set; }

        /// <summary>
        /// type
        /// Required: true
        /// Enum: [&quot;tcp&quot;,&quot;udp&quot;,&quot;sctp&quot;]
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;
    }
}
