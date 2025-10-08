using System;
using Microsoft.Extensions.Logging;

public interface IDockerHandlerFactory
{
    Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger);
}
