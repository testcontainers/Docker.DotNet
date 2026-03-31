#nullable enable
namespace Docker.DotNet.Models
{
    public class CopyToContainerParameters // (main.CopyToContainerParameters)
    {
        [QueryStringParameter("path", true)]
        public string Path { get; set; } = default!;

        [QueryStringParameter<BoolQueryStringConverter>("noOverwriteDirNonDir", false)]
        public bool? AllowOverwriteDirWithFile { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("copyUIDGID", false)]
        public bool? CopyUIDGID { get; set; }
    }
}
