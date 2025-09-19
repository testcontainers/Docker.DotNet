namespace Docker.DotNet.Models
{
    public class DeviceInfo // (system.DeviceInfo)
    {
        [JsonPropertyName("Source")]
        public string Source { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }
    }
}
