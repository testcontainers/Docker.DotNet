#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageDeleteParameters // (main.ImageDeleteParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("force", false)]
        public bool? Force { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("noprune", false)]
        public bool? NoPrune { get; set; }
    }
}
