
namespace Docker.DotNet.Models
{
    public class PluginGetPrivilegeParameters // (main.PluginGetPrivilegeParameters)
    {
        [QueryStringParameter("remote", true)]
        public string Remote { get; set; }
    }
}
