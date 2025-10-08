using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Docker.DotNet.NPipe
{
    public class NpipeHandlerFactory : IDockerHandlerFactory
    {
        public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger)
        {
            if (configuration.Credentials.IsTlsCredentials())
            {
                throw new Exception("TLS not supported over npipe");
            }
            var segments = uri.Segments;
            if (segments.Length != 3 || !segments[1].Equals("pipe/", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{configuration.EndpointBaseUri} is not a valid npipe URI");
            }
            var serverName = uri.Host;
            if (string.Equals(serverName, "localhost", StringComparison.OrdinalIgnoreCase))
            {
                serverName = ".";
            }
            var pipeName = uri.Segments[2];
            uri = new UriBuilder("http", pipeName).Uri;
            
            return new Tuple<HttpMessageHandler, Uri>(
                new Microsoft.Net.Http.Client.ManagedHandler(async (host, port, cancellationToken) =>
                {
                    var timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                    var stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                    var dockerStream = new DockerPipeStream(stream);
                    await stream.ConnectAsync(timeout, cancellationToken).ConfigureAwait(false);
                    return dockerStream;
                }, logger),
                uri
            );
        }
    }
}
