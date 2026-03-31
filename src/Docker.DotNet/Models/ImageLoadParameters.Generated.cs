#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageLoadParameters // (main.ImageLoadParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("quiet", true)]
        public bool Quiet { get; set; } = default!;
    }
}
