#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageLoadParameters // (main.ImageLoadParameters)
    {
        [QueryStringParameter("quiet", true, typeof(QueryStringBoolConverter))]
        public bool Quiet { get; set; } = default!;
    }
}
