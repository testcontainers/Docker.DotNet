
namespace Docker.DotNet.Models
{
    public class ContainerRestartParameters // (main.ContainerRestartParameters)
    {
        [QueryStringParameter("t", false)]
        public uint? WaitBeforeKillSeconds { get; set; }

        [QueryStringParameter("signal", false)]
        public string Signal { get; set; }
    }
}
