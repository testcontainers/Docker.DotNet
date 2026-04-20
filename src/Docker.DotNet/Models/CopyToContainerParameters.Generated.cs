#nullable enable
namespace Docker.DotNet.Models
{
    public class CopyToContainerParameters // (main.CopyToContainerParameters)
    {
        [QueryStringParameter("path", true)]
        public string Path { get; set; } = default!;

        [QueryStringParameter<QueryStringBoolConverter>("noOverwriteDirNonDir", false)]
        public bool? AllowOverwriteDirWithFile { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("copyUIDGID", false)]
        public bool? CopyUIDGID { get; set; }
    }
}
