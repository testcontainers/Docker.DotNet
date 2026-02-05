namespace Docker.DotNet.Tests;

using System.Net.Http;
using Docker.DotNet.Handler.Abstractions;

public class DockerClientConfigurationTests
{
    [Fact]
    public void DefaultConstructor_UsesDefaultSocketConfiguration()
    {
        var config = new DockerClientConfiguration(new Uri("unix:/var/run/docker.sock"));

        Assert.Same(SocketConnectionConfiguration.Default, config.SocketConfiguration);
    }

    [Fact]
    public void Constructor_AcceptsCustomSocketConfiguration()
    {
        var socketConfig = new SocketConnectionConfiguration
        {
            KeepAlive = false,
            NoDelay = false,
        };

        var config = new DockerClientConfiguration(
            new Uri("unix:/var/run/docker.sock"),
            socketConfiguration: socketConfig);

        Assert.Same(socketConfig, config.SocketConfiguration);
        Assert.False(config.SocketConfiguration.KeepAlive);
        Assert.False(config.SocketConfiguration.NoDelay);
    }

    [Fact]
    public void Constructor_AcceptsConfigureHandlerCallback()
    {
        var callbackInvoked = false;

        var config = new DockerClientConfiguration(
            new Uri("unix:/var/run/docker.sock"),
            configureHandler: _ => callbackInvoked = true);

        Assert.NotNull(config.ConfigureHandler);
        config.ConfigureHandler(new HttpClientHandler());
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void Constructor_ConfigureHandlerDefaultsToNull()
    {
        var config = new DockerClientConfiguration(new Uri("unix:/var/run/docker.sock"));

        Assert.Null(config.ConfigureHandler);
    }

    [Fact]
    public void DefaultTimeout_HasExpectedDefault()
    {
        var config = new DockerClientConfiguration(new Uri("unix:/var/run/docker.sock"));

        Assert.Equal(TimeSpan.FromSeconds(100), config.DefaultTimeout);
    }

    [Fact]
    public void NamedPipeConnectTimeout_HasExpectedDefault()
    {
        var config = new DockerClientConfiguration(new Uri("unix:/var/run/docker.sock"));

        Assert.Equal(TimeSpan.FromMilliseconds(100), config.NamedPipeConnectTimeout);
    }
}
