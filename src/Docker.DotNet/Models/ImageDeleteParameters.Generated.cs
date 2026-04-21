#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageDeleteParameters // (main.ImageDeleteParameters)
    {
        [QueryStringBoolParameter("force", false)]
        public bool? Force { get; set; }

        [QueryStringBoolParameter("noprune", false)]
        public bool? NoPrune { get; set; }
    }
}
