#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageDeleteParameters // (main.ImageDeleteParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("force", false)]
        public bool? Force { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("noprune", false)]
        public bool? NoPrune { get; set; }
    }
}
