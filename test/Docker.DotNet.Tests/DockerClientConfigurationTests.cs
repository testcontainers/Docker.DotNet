namespace Docker.DotNet.Tests;

public class DockerClientConfigurationTests
{
    [Fact]
    public void SocketConnectTimeout_DefaultValue_Is30Seconds()
    {
        using var config = new DockerClientConfiguration();

        Assert.Equal(TimeSpan.FromSeconds(30), config.SocketConnectTimeout);
    }

    [Fact]
    public void SocketConnectTimeout_CustomValue_IsPreserved()
    {
        var customTimeout = TimeSpan.FromSeconds(60);

        using var config = new DockerClientConfiguration(
            socketConnectTimeout: customTimeout);

        Assert.Equal(customTimeout, config.SocketConnectTimeout);
    }

    [Fact]
    public void NamedPipeConnectTimeout_DefaultValue_Is100Milliseconds()
    {
        using var config = new DockerClientConfiguration();

        Assert.Equal(TimeSpan.FromMilliseconds(100), config.NamedPipeConnectTimeout);
    }

    [Fact]
    public void DefaultTimeout_DefaultValue_Is100Seconds()
    {
        using var config = new DockerClientConfiguration();

        Assert.Equal(TimeSpan.FromSeconds(100), config.DefaultTimeout);
    }
}
