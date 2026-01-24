namespace Docker.DotNet.Models
{
    public class JSONError // (jsonstream.Error)
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
