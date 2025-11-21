namespace Docker.DotNet.Models
{
    public class Privilege // (plugin.Privilege)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Value")]
        public IList<string> Value { get; set; }
    }
}
