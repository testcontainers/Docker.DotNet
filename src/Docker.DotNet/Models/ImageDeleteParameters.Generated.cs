#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageDeleteParameters // (main.ImageDeleteParameters)
    {
        [QueryStringParameter("force", false, typeof(QueryStringBoolConverter))]
        public bool? Force { get; set; }

        [QueryStringParameter("noprune", false, typeof(QueryStringBoolConverter))]
        public bool? NoPrune { get; set; }
    }
}
