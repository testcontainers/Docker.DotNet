#nullable enable
namespace Docker.DotNet.Models
{
    public class CopyToContainerParameters // (main.CopyToContainerParameters)
    {
        [QueryStringParameter("path", true)]
        public string Path { get; set; } = string.Empty;

        [QueryStringBoolParameter("noOverwriteDirNonDir", false)]
        public bool? AllowOverwriteDirWithFile { get; set; }

        [QueryStringBoolParameter("copyUIDGID", false)]
        public bool? CopyUIDGID { get; set; }
    }
}
