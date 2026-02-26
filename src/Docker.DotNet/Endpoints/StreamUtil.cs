namespace Docker.DotNet.Models;

internal static class StreamUtil
{
    internal static async Task MonitorStreamAsync(MultiplexedStream multiplexedStream, IProgress<string> progress, CancellationToken cancellationToken = default)
    {
        var buffer = new byte[8192];

        using var memoryStream = new MemoryStream();

        using var streamReader = new StreamReader(memoryStream, new UTF8Encoding(false), false, buffer.Length, true);

        while (true)
        {
            // TODO:
            // Make sure that split multi-byte characters across read operations don't
            // cause exceptions or get misinterpreted.

            var bytesRead = await multiplexedStream.ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken)
                .ConfigureAwait(false);

            if (bytesRead.Count == 0)
            {
                break;
            }

            await memoryStream.WriteAsync(buffer, 0, bytesRead.Count, cancellationToken)
                .ConfigureAwait(false);

            // Read each line from the stream. If no complete line is available, it may be because
            // the line is incomplete and the remaining bytes will be read in the next operation.
            // We need to retain any leftover bytes to process them in the next iteration.

            memoryStream.Seek(0, SeekOrigin.Begin);

            long lastReadPosition;

            while (true)
            {
                var line = await streamReader.ReadLineAsync()
                    .ConfigureAwait(false);

                if (line == null)
                {
                    lastReadPosition = memoryStream.Position;
                    break;
                }
                else
                {
                    progress.Report(line);
                }
            }

            var remainingBytes = memoryStream.Length - lastReadPosition;

            if (remainingBytes > 0)
            {
                var internalBuffer = memoryStream.GetBuffer();
                Array.Copy(internalBuffer, lastReadPosition, internalBuffer, 0, remainingBytes);
            }

            memoryStream.Position = remainingBytes;
            memoryStream.SetLength(remainingBytes);
        }
    }

    internal static async Task MonitorStreamForMessagesAsync<T>(Task<StandardStreamResponse> responseTask, IProgress<T> progress, CancellationToken cancellationToken)
    {
        using var response = await responseTask
            .ConfigureAwait(false);

        var asyncEnumerable = DockerClient.JsonSerializer.DeserializeAsync<T>(response, cancellationToken)
            .ConfigureAwait(false);

        await foreach (var value in asyncEnumerable)
        {
            progress.Report(value);
        }
    }

    internal static async Task MonitorResponseForMessagesAsync<T>(Task<HttpResponseMessage> responseTask, IProgress<T> progress, CancellationToken cancellationToken)
    {
        using var response = await responseTask
            .ConfigureAwait(false);

        var stream = await response.Content.ReadAsStreamAsync()
            .ConfigureAwait(false);

        await MonitorStreamForMessagesAsync(Task.FromResult(new StandardStreamResponse(response, stream)), progress, cancellationToken)
            .ConfigureAwait(false);
    }
}