namespace Docker.DotNet.Models
{
    public class ContainerdInfo // (system.ContainerdInfo)
    {
        [JsonPropertyName("Address")]
        public string Address { get; set; }

        [JsonPropertyName("Namespaces")]
        public ContainerdNamespaces Namespaces { get; set; }
    }
}
