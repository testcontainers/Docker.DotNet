namespace Docker.DotNet.Tests;

public sealed class LogsResponseFramingTests
{
    private const string LogsResponseHeaders = "HTTP/1.1 200 OK\r\nApi-Version: 1.47\r\nContent-Type: application/vnd.docker.multiplexed-stream\r\n";

    private const string LogsResponseHeadersWithoutSpaces = "HTTP/1.1 200 OK\r\nApi-Version:1.47\r\nContent-Type:application/vnd.docker.multiplexed-stream\r\n";

    // One 14-byte stdout frame: stream type 1, three padding bytes,
    // a big-endian length of 6, then the payload.
    private const string StdoutFrame = "\u0001\0\0\0\0\0\0\u0006hello\n";

    [Theory]
    [InlineData(LogsResponseHeaders + "Transfer-Encoding: chunked\r\n\r\n0\r\n\r\n", "", Label = "chunked, empty")]
    [InlineData(LogsResponseHeaders + "Transfer-Encoding: chunked\r\n\r\ne\r\n" + StdoutFrame + "\r\n0\r\n\r\n", "hello\n", Label = "chunked, one frame")]
    [InlineData(LogsResponseHeaders + "Content-Length: 0\r\n\r\n", "", Label = "Content-Length, empty")]
    [InlineData(LogsResponseHeaders + "Content-Length: 14\r\n\r\n" + StdoutFrame, "hello\n", Label = "Content-Length, one frame")]
    [InlineData(LogsResponseHeaders + "Connection: close\r\n\r\n", "", Label = "close-delimited, empty")]
    [InlineData(LogsResponseHeaders + "Connection: close\r\n\r\n" + StdoutFrame, "hello\n", Label = "close-delimited, one frame")]
    [InlineData(LogsResponseHeadersWithoutSpaces + "Transfer-Encoding:chunked\r\n\r\ne\r\n" + StdoutFrame + "\r\n0\r\n\r\n", "hello\n", Label = "chunked, no spaces after header colons")]
    [InlineData(LogsResponseHeaders + "Content-Length:\t 14 \t\r\n\r\n" + StdoutFrame, "hello\n", Label = "Content-Length, tabs and spaces around header value")]
    public async Task GetContainerLogsAsync_ResponseFraming_ReadsLikeNativeTransport(string response, string expectedStdout)
    {
        var legacyStdout = await ReadLogsAsync(LegacyHttp.DockerHandlerFactory.Instance, new LegacyHttpTransportOptions(), response);
        var nativeStdout = await ReadLogsAsync(NativeHttp.DockerHandlerFactory.Instance, new NativeHttpTransportOptions(), response);

        Assert.Equal(expectedStdout, nativeStdout);
        Assert.Equal(nativeStdout, legacyStdout);
    }

    [Theory]
    [InlineData(LogsResponseHeaders + "Transfer-Encoding: chunked\r\n\r\ne\r\n" + StdoutFrame + "\r\n", "The connection closed before the next chunk header.", Label = "before next chunk header")]
    [InlineData(LogsResponseHeaders + "Transfer-Encoding: chunked\r\n\r\ne\r\n\u0001\0\0\0", "The connection closed in the middle of a chunk.", Label = "in middle of chunk")]
    [InlineData(LogsResponseHeaders + "Transfer-Encoding: chunked\r\n\r\ne\r\n" + StdoutFrame, "The connection closed before the empty line ending a chunk.", Label = "before empty line ending chunk")]
    public async Task GetContainerLogsAsync_TruncatedChunkedResponse_ThrowsIOException(string response, string expectedMessage)
    {
        var legacyException = await Assert.ThrowsAsync<EndOfStreamException>(() => ReadLogsAsync(LegacyHttp.DockerHandlerFactory.Instance, new LegacyHttpTransportOptions(), response));
        await Assert.ThrowsAnyAsync<IOException>(() => ReadLogsAsync(NativeHttp.DockerHandlerFactory.Instance, new NativeHttpTransportOptions(), response));

        Assert.Equal(expectedMessage, legacyException.Message);
    }

    private static async Task<string> ReadLogsAsync<TTransportOptions>(IDockerHandlerFactory<TTransportOptions> transportFactory, TTransportOptions transportOptions, string response)
    {
        var parameters = new ContainerLogsParameters
        {
            ShowStderr = true,
            ShowStdout = true
        };

        await using var daemon = new FakeDaemon(response);

        using var client = new DockerClientBuilder()
            .WithEndpoint(daemon.Endpoint)
            .WithTransportOptions(transportFactory, transportOptions)
            .Build();

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(10));

        using var multiplexedStream = await client.Containers.GetContainerLogsAsync("x", parameters, cts.Token)
            .ConfigureAwait(false);

        var (stdout, _) = await multiplexedStream.ReadOutputToEndAsync(cts.Token)
            .ConfigureAwait(false);

        return stdout;
    }

    // Answers /logs with the given response,
    // and the container inspect with a non-TTY container.
    private sealed class FakeDaemon : IAsyncDisposable
    {
        private const string InspectJson = "{\"Id\":\"x\",\"Config\":{\"Tty\":false}}";

        private static readonly byte[] InspectResponse = Encoding.Latin1.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: {InspectJson.Length}\r\nConnection: close\r\n\r\n{InspectJson}");

        private readonly TcpListener _listener = new(IPAddress.Loopback, 0);

        private readonly CancellationTokenSource _stopping = new();

        private readonly Task _serving;

        public FakeDaemon(string logsResponse)
        {
            _listener.Start();

            // Latin-1 maps each char to the byte with the same value, so frame
            // bytes above 0x7F are sent unchanged.
            _serving = ServeAsync(Encoding.Latin1.GetBytes(logsResponse));
            Endpoint = new Uri($"tcp://127.0.0.1:{((IPEndPoint)_listener.LocalEndpoint).Port}");
        }

        public Uri Endpoint { get; }

        public async ValueTask DisposeAsync()
        {
            _stopping.Cancel();

            try
            {
                await _serving
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // This is expected when the daemon is disposed.
            }
            finally
            {
                _listener.Stop();
                _stopping.Dispose();
            }
        }

        private async Task ServeAsync(byte[] logsResponse)
        {
            while (true)
            {
                using var client = await _listener.AcceptTcpClientAsync(_stopping.Token)
                    .ConfigureAwait(false);

                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, Encoding.Latin1);

                var requestLine = await reader.ReadLineAsync(_stopping.Token)
                    .ConfigureAwait(false);

                // Reads the remaining request headers,
                // so closing the connection doesn't reset it.
                do
                {
                    var header = await reader.ReadLineAsync(_stopping.Token)
                        .ConfigureAwait(false);

                    if (string.IsNullOrEmpty(header))
                    {
                        break;
                    }
                }
                while (true);

                var response = requestLine?.Contains("/logs") == true ? logsResponse : InspectResponse;

                await stream.WriteAsync(response, _stopping.Token)
                    .ConfigureAwait(false);
            }
        }
    }
}