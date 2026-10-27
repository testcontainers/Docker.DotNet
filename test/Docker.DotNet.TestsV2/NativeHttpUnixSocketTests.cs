using System.Net.Sockets;
using System.Threading;

namespace Docker.DotNet.TestsV2;

public sealed class NativeHttpUnixSocketTests
{
    [Fact]
    public void CreateHandler_ConnectsToTheSocket_WhenEndpointSchemeIsUnix()
    {
        var clientOptions = new ResolvedClientOptions { Endpoint = new Uri("unix:///var/run/docker.sock") };

        var transport = NativeHttp.DockerHandlerFactory.Instance.CreateHandler(clientOptions, NullLogger.Instance);

        var handler = Assert.IsType<SocketsHttpHandler>(transport.Handler);
        Assert.NotNull(handler.ConnectCallback);
        Assert.Equal(new Uri("http://docker.sock/"), transport.EffectiveEndpoint);
    }

    [Fact]
    public void CreateHandler_AppliesConfigureHandlerAfterTheSocketConnectCallback()
    {
        Func<SocketsHttpConnectionContext, CancellationToken, ValueTask<Stream>> connectCallback = (_, _) => throw new NotSupportedException();
        var transportOptions = new NativeHttpTransportOptions { ConfigureHandler = handler => handler.ConnectCallback = connectCallback };
        var clientOptions = new ResolvedClientOptions { Endpoint = new Uri("unix:///var/run/docker.sock") };

        var transport = NativeHttp.DockerHandlerFactory.Instance.CreateHandler(transportOptions, clientOptions, NullLogger.Instance);

        var handler = Assert.IsType<SocketsHttpHandler>(transport.Handler);
        Assert.Same(connectCallback, handler.ConnectCallback);
    }

    [Fact]
    public async Task GetVersionAsync_ReadsTheResponse_FromADaemonOnAUnixSocket()
    {
        var socketPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.sock");

        using var listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
        listener.Bind(new UnixDomainSocketEndPoint(socketPath));
        listener.Listen(1);

        try
        {
            var serving = ServeOneRequestAsync(listener);

            using var client = new DockerClientBuilder()
                .WithEndpoint(new Uri("unix://" + socketPath))
                .WithTransportOptions(new NativeHttpTransportOptions())
                .Build();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var version = await client.System.GetVersionAsync(cts.Token);
            var requestLine = await serving;

            Assert.Equal("29.0.0", version.Version);
            Assert.EndsWith("/version HTTP/1.1", requestLine, StringComparison.Ordinal);
        }
        finally
        {
            File.Delete(socketPath);
        }
    }

    private static async Task<string> ServeOneRequestAsync(Socket listener)
    {
        const string body = "{\"Version\":\"29.0.0\",\"ApiVersion\":\"1.52\",\"MinAPIVersion\":\"1.24\"}";

        using var connection = await listener.AcceptAsync();
        using var stream = new NetworkStream(connection, ownsSocket: false);

        var request = new StringBuilder();
        var buffer = new byte[4096];
        while (!request.ToString().Contains("\r\n\r\n"))
        {
            var read = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
            {
                break;
            }

            request.Append(Encoding.ASCII.GetString(buffer, 0, read));
        }

        var response = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n{body}");
        await stream.WriteAsync(response, 0, response.Length);
        connection.Shutdown(SocketShutdown.Both);

        return request.ToString().Split(["\r\n"], StringSplitOptions.None)[0];
    }
}
