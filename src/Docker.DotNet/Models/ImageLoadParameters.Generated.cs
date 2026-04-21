#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageLoadParameters // (main.ImageLoadParameters)
    {
        [QueryStringBoolParameter("quiet", true)]
        public bool Quiet { get; set; } = default!;
    }
}
