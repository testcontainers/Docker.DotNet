#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Topology is a map of topological domains to topological segments.
    /// 
    /// This description is taken verbatim from the CSI Spec:
    /// 
    /// A topological domain is a sub-division of a cluster, like &quot;region&quot;,
    /// &quot;zone&quot;, &quot;rack&quot;, etc.
    /// A topological segment is a specific instance of a topological domain,
    /// like &quot;zone3&quot;, &quot;rack3&quot;, etc.
    /// For example {&quot;com.company/zone&quot;: &quot;Z1&quot;, &quot;com.company/rack&quot;: &quot;R3&quot;}
    /// Valid keys have two segments: an OPTIONAL prefix and name, separated
    /// by a slash (/), for example: &quot;com.company.example/zone&quot;.
    /// The key name segment is REQUIRED. The prefix is OPTIONAL.
    /// The key name MUST be 63 characters or less, begin and end with an
    /// alphanumeric character ([a-z0-9A-Z]), and contain only dashes (-),
    /// underscores (_), dots (.), or alphanumerics in between, for example
    /// &quot;zone&quot;.
    /// The key prefix MUST be 63 characters or less, begin and end with a
    /// lower-case alphanumeric character ([a-z0-9]), contain only
    /// dashes (-), dots (.), or lower-case alphanumerics in between, and
    /// follow domain name notation format
    /// (https://tools.ietf.org/html/rfc1035#section-2.3.1).
    /// The key prefix SHOULD include the plugin&apos;s host company name and/or
    /// the plugin name, to minimize the possibility of collisions with keys
    /// from other plugins.
    /// If a key prefix is specified, it MUST be identical across all
    /// topology keys returned by the SP (across all RPCs).
    /// Keys MUST be case-insensitive. Meaning the keys &quot;Zone&quot; and &quot;zone&quot;
    /// MUST not both exist.
    /// Each value (topological segment) MUST contain 1 or more strings.
    /// Each string MUST be 63 characters or less and begin and end with an
    /// alphanumeric character with &apos;-&apos;, &apos;_&apos;, &apos;.&apos;, or alphanumerics in
    /// between.
    /// </summary>
    public class VolumeTopology // (volume.Topology)
    {
        [JsonPropertyName("Segments")]
        public IDictionary<string, string>? Segments { get; set; }
    }
}
