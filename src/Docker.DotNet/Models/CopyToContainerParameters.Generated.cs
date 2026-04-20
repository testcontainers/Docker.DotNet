#nullable enable
namespace Docker.DotNet.Models
{
    public class CopyToContainerParameters // (main.CopyToContainerParameters)
    {
        [QueryStringParameter("path", true)]
        public string Path { get; set; } = default!;

        [QueryStringParameter("noOverwriteDirNonDir", false, typeof(QueryStringBoolConverter))]
        public bool? AllowOverwriteDirWithFile { get; set; }

        [QueryStringParameter("copyUIDGID", false, typeof(QueryStringBoolConverter))]
        public bool? CopyUIDGID { get; set; }
    }
}
