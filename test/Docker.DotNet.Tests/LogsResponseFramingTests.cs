using System.Net;
using System.Net.Sockets;

namespace Docker.DotNet.Tests;

/// <summary>
/// Serves <c>/containers/{id}/logs</c> responses in each framing HTTP allows from a fake daemon, and reads them through
/// the legacy and native transports. Both must read the same output: the framing comes from <c>Transfer-Encoding</c>
/// or <c>Content-Length</c>, not from Docker's stream content type.
/// </summary>
public sealed class LogsResponseFramingTests
{
    private const string Head = "HTTP/1.1 200 OK\r\nApi-Version: 1.47\r\nContent-Type: application/vnd.docker.multiplexed-stream\r\n";

    // One stdout frame: stream type 1, three padding bytes, a big-endian length of 6, then the payload.
    private const string Frame = "\u0001\0\0\0\0\0\0\u0006hello\n";

    private static readonly Dictionary<string, (string Response, string Stdout)> Framings = new()
    {
        ["chunked, empty"] = (Head + "Transfer-Encoding: chunked\r\n\r\n0\r\n\r\n", ""),
        ["chunked, one frame"] = (Head + "Transfer-Encoding: chunked\r\n\r\ne\r\n" + Frame + "\r\n0\r\n\r\n", "hello\n"),
        ["Content-Length 0"] = (Head + "Content-Length: 0\r\n\r\n", ""),
        ["Content-Length, one frame"] = (Head + "Content-Length: 14\r\n\r\n" + Frame, "hello\n"),
        ["close-delimited, empty"] = (Head + "Connection: close\r\n\r\n", ""),
        ["close-delimited, one frame"] = (Head + "Connection: close\r\n\r\n" + Frame, "hello\n"),
        ["chunked, no space after the header colons"] = ("HTTP/1.1 200 OK\r\nContent-Type:application/vnd.docker.multiplexed-stream\r\nTransfer-Encoding:chunked\r\n\r\ne\r\n" + Frame + "\r\n0\r\n\r\n", "hello\n"),
        ["Content-Length, tabs and spaces around the header value"] = (Head + "Content-Length:\t 14 \t\r\n\r\n" + Frame, "hello\n"),
    };

    [Theory]
    [InlineData("chunked, empty")]
    [InlineData("chunked, one frame")]
    [InlineData("Content-Length 0")]
    [InlineData("Content-Length, one frame")]
    [InlineData("close-delimited, empty")]
    [InlineData("close-delimited, one frame")]
    [InlineData("chunked, no space after the header colons")]
    [InlineData("Content-Length, tabs and spaces around the header value")]
    public async Task GetContainerLogsAsync_ResponseFraming_ReadsLikeNativeTransport(string framing)
    {
        var (response, expectedStdout) = Framings[framing];

        var legacy = await ReadLogsAsync(new LegacyHttpTransportOptions(), response);
        var native = await ReadLogsAsync(new NativeHttpTransportOptions(), response);

        Assert.Equal(expectedStdout, native);
        Assert.Equal(native, legacy);
    }

    [Theory]
    [InlineData(Head + "Transfer-Encoding: chunked\r\n\r\ne\r\n" + Frame + "\r\n", "The connection closed before the next chunk header.")]
    [InlineData(Head + "Transfer-Encoding: chunked\r\n\r\ne\r\n\u0001\0\0\0", "The connection closed in the middle of a chunk.")]
    [InlineData(Head + "Transfer-Encoding: chunked\r\n\r\ne\r\n" + Frame, "The connection closed before the empty line ending a chunk.")]
    public async Task GetContainerLogsAsync_TruncatedChunkedResponse_ThrowsIOException(string response, string legacyMessage)
    {
        var legacy = await Assert.ThrowsAsync<EndOfStreamException>(() => ReadLogsAsync(new LegacyHttpTransportOptions(), response));
        await Assert.ThrowsAnyAsync<IOException>(() => ReadLogsAsync(new NativeHttpTransportOptions(), response));

        Assert.Equal(legacyMessage, legacy.Message);
    }

    private static Task<string> ReadLogsAsync(LegacyHttpTransportOptions transportOptions, string response)
    {
        return ReadLogsAsync(endpoint => new DockerClientBuilder().WithEndpoint(endpoint).WithTransportOptions(transportOptions).Build(), response);
    }

    private static Task<string> ReadLogsAsync(NativeHttpTransportOptions transportOptions, string response)
    {
        return ReadLogsAsync(endpoint => new DockerClientBuilder().WithEndpoint(endpoint).WithTransportOptions(transportOptions).Build(), response);
    }

    private static async Task<string> ReadLogsAsync(Func<Uri, DockerClient> createClient, string response)
    {
        await using var daemon = new FakeDaemon(response);
        using var client = createClient(daemon.Endpoint);
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(10));

        using var logs = await client.Containers.GetContainerLogsAsync("x", new ContainerLogsParameters { ShowStdout = true, ShowStderr = true }, cts.Token);
        var (stdout, _) = await logs.ReadOutputToEndAsync(cts.Token);
        return stdout;
    }

    /// <summary>
    /// Answers <c>/logs</c> with a fixed raw response, and every other request (the client inspects the container to
    /// learn whether it has a TTY) with a minimal container JSON. It closes each connection after one response.
    /// </summary>
    private sealed class FakeDaemon : IAsyncDisposable
    {
        private const string Inspect = "{\"Id\":\"x\",\"Config\":{\"Tty\":false}}";

        private readonly TcpListener _listener = new(IPAddress.Loopback, 0);

        private readonly CancellationTokenSource _stopping = new();

        private readonly Task _serving;

        public FakeDaemon(string logsResponse)
        {
            _listener.Start();
            Endpoint = new Uri($"tcp://127.0.0.1:{((IPEndPoint)_listener.LocalEndpoint).Port}");
            _serving = ServeAsync(logsResponse);
        }

        public Uri Endpoint { get; }

        public async ValueTask DisposeAsync()
        {
            _stopping.Cancel();

            try
            {
                await _serving.ConfigureAwait(false);
            }
            finally
            {
                _listener.Stop();
                _stopping.Dispose();
            }
        }

        private async Task ServeAsync(string logsResponse)
        {
            while (true)
            {
                TcpClient client;
                try
                {
                    client = await _listener.AcceptTcpClientAsync(_stopping.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                using (client)
                {
                    var stream = client.GetStream();
                    var request = new StringBuilder();
                    var buffer = new byte[4096];
                    while (!request.ToString().Contains("\r\n\r\n"))
                    {
                        var read = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                        if (read == 0)
                        {
                            break;
                        }

                        request.Append(Encoding.ASCII.GetString(buffer, 0, read));
                    }

                    var requestLine = request.ToString().Split(["\r\n"], StringSplitOptions.None)[0];
                    var reply = requestLine.Contains("/logs")
                        ? logsResponse
                        : $"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: {Inspect.Length}\r\nConnection: close\r\n\r\n{Inspect}";

                    // Latin-1 maps each char to one byte, so the frame header's control bytes go out unchanged.
                    var bytes = Encoding.Latin1.GetBytes(reply);
                    await stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                    client.Client.Shutdown(SocketShutdown.Both);
                }
            }
        }
    }
}
