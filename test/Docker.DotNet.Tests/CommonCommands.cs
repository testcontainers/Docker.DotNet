namespace Docker.DotNet.Tests;

public static class CommonCommands
{
    public static readonly string[] SleepInfinity = ["/bin/sh", "-c", "trap \"exit 0\" TERM INT; sleep infinity"];

    public static readonly string[] EchoToStdoutAndStderr = ["/bin/sh", "-c", "trap \"exit 0\" TERM INT; while true; do echo \"stdout message\"; echo \"stderr message\" >&2; sleep 1; done"];
}