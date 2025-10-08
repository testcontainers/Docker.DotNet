namespace Docker.DotNet.Tests;

public static class CommonCommands
{
    public static readonly string[] SleepInfinity = ["/bin/sh", "-c", "trap \"exit 0\" TERM INT; sleep infinity"];

    public static readonly string[] EchoToStdoutAndStderr = ["/bin/sh", "-c", "trap \"exit 0\" TERM INT; RND=$RANDOM; while true; do echo \"stdout message $RND\"; echo \"stderr message $RND\" >&2; sleep 1; done"];

    public static readonly string[] EchoToStdoutAndStderrFast = ["/bin/sh", "-c", "trap \"exit 0\" TERM INT; RND=$RANDOM; while true; do echo \"stdout message $RND\"; echo \"stderr message $RND\" >&2; done"];
}