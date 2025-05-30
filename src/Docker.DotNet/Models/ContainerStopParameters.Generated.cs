
namespace Docker.DotNet.Models
{
    public class ContainerStopParameters // (main.ContainerStopParameters)
    {
        [QueryStringParameter("t", false)]
        public uint? WaitBeforeKillSeconds { get; set; }

        [QueryStringParameter("signal", false)]
        public string Signal { get; set; }
    }
}
