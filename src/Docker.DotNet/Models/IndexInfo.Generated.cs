#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IndexInfo contains information about a registry
    /// 
    /// RepositoryInfo Examples:
    /// 
    /// 	{
    /// 	  &quot;Index&quot; : {
    /// 	    &quot;Name&quot; : &quot;docker.io&quot;,
    /// 	    &quot;Mirrors&quot; : [&quot;https://registry-2.docker.io/v1/&quot;, &quot;https://registry-3.docker.io/v1/&quot;],
    /// 	    &quot;Secure&quot; : true,
    /// 	    &quot;Official&quot; : true,
    /// 	  },
    /// 	  &quot;RemoteName&quot; : &quot;library/debian&quot;,
    /// 	  &quot;LocalName&quot; : &quot;debian&quot;,
    /// 	  &quot;CanonicalName&quot; : &quot;docker.io/debian&quot;
    /// 	  &quot;Official&quot; : true,
    /// 	}
    /// 
    /// 	{
    /// 	  &quot;Index&quot; : {
    /// 	    &quot;Name&quot; : &quot;127.0.0.1:5000&quot;,
    /// 	    &quot;Mirrors&quot; : [],
    /// 	    &quot;Secure&quot; : false,
    /// 	    &quot;Official&quot; : false,
    /// 	  },
    /// 	  &quot;RemoteName&quot; : &quot;user/repo&quot;,
    /// 	  &quot;LocalName&quot; : &quot;127.0.0.1:5000/user/repo&quot;,
    /// 	  &quot;CanonicalName&quot; : &quot;127.0.0.1:5000/user/repo&quot;,
    /// 	  &quot;Official&quot; : false,
    /// 	}
    /// </summary>
    public class IndexInfo // (registry.IndexInfo)
    {
        /// <summary>
        /// Name is the name of the registry, such as &quot;docker.io&quot;
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Mirrors is a list of mirrors, expressed as URIs
        /// </summary>
        [JsonPropertyName("Mirrors")]
        public IList<string> Mirrors { get; set; } = default!;

        /// <summary>
        /// Secure is set to false if the registry is part of the list of
        /// insecure registries. Insecure registries accept HTTP and/or accept
        /// HTTPS with certificates from unknown CAs.
        /// </summary>
        [JsonPropertyName("Secure")]
        public bool Secure { get; set; } = default!;

        /// <summary>
        /// Official indicates whether this is an official registry
        /// </summary>
        [JsonPropertyName("Official")]
        public bool Official { get; set; } = default!;
    }
}
