> [!IMPORTANT]
> Unfortunately, there has been no further development or bug fixes in [Docker.DotNet](https://github.com/dotnet/Docker.DotNet/tree/aacf3c26131f582ca8acc34084663a4b79e28d38) for some time now, and the repository appears inactive. Reaching the current maintainer is challenging. I understand that priorities shift, and maintaining an open-source project is time-consuming and exhausting. As maintainers, we often cannot dedicate the necessary time. Over the past months—and even years—I have frequently offered my help.
>
> Docker.DotNet is an important upstream dependency for Testcontainers for .NET and many other developers. It would be unfortunate for this valuable work not to continue. Therefore, I have decided to fork the repository to focus on essential fixes, improvements, and updates for the Docker Engine API. I am also accepting further contributions and remaining PRs from the upstream repository.

# .NET Client for Docker Remote API

This library allows you to interact with [Docker Remote API][docker-remote-api] endpoints in your .NET applications.

It is fully asynchronous, designed to be non-blocking and object-oriented way to interact with your Docker daemon programmatically.

## Installation

[![NuGet latest release](https://img.shields.io/nuget/v/Docker.DotNet.Enhanced.svg)](https://www.nuget.org/packages/Docker.DotNet.Enhanced)

You can add this library to your project using [NuGet][nuget].

**Package Manager Console**

Run the following command in the "Package Manager Console":

```console
PM> Install-Package Docker.DotNet.Enhanced
```

**Visual Studio**

Right click to your project in Visual Studio, choose "Manage NuGet Packages" and search for "Docker.DotNet.Enhanced" and click "Install" (see [NuGet Gallery][nuget-gallery]).

**.NET Core Command Line Interface**

Run the following command from your favorite shell or terminal:

```console
dotnet add package Docker.DotNet.Enhanced
```

**Development Builds**

[![CI](https://github.com/testcontainers/Docker.DotNet/actions/workflows/ci.yml/badge.svg)](https://github.com/testcontainers/Docker.DotNet/actions/workflows/ci.yml)

## Usage

You can initialize the client as follows:

```csharp
using Docker.DotNet;
var client = new DockerClientBuilder()
    .WithEndpoint(new Uri("http://ubuntu-docker.cloudapp.net:4243"))
    .Build();
```

**Named Pipe (Windows):**

```csharp
using Docker.DotNet;
var client = new DockerClientBuilder()
    .WithEndpoint(new Uri("npipe://./pipe/docker_engine"))
    .Build();
```

**Unix Domain Socket (Linux/macOS):**

```csharp
using Docker.DotNet;
var client = new DockerClientBuilder()
    .WithEndpoint(new Uri("unix:/var/run/docker.sock"))
    .Build();
```

**Note:**
For HTTP(S) connections or special authentication types (e.g. X509, BasicAuth), see the corresponding sections below.

To connect to your local [Docker Desktop on Windows](https://docs.docker.com/desktop/setup/install/windows-install/) instance via named pipe or your local [Docker Desktop on Mac](https://docs.docker.com/desktop/setup/install/mac-install/) instance via Unix socket:

```csharp
using Docker.DotNet;
var client = new DockerClientBuilder()
    .Build();
```

### Enabling .NET Native HTTP Handler

**Experimental**: To enhance the communication between your application and the Docker Engine, you can enable the .NET native (socket) HTTP handler.
This is achieved by setting the environment variable `DOCKER_DOTNET_NATIVE_HTTP_ENABLED` to `1`.
By doing so, the handler used to communicate with the Docker Engine will switch to the .NET built-in one, potentially improving performance and reliability.

### Transport-specific options

Use `WithTransportOptions` to select a transport explicitly and configure transport-specific options.

For example, on Windows named pipes you can configure the pipe connect timeout:

```csharp
using Docker.DotNet;
using Docker.DotNet.NPipe;

var client = new DockerClientBuilder()
    .WithTransportOptions(new NPipeTransportOptions
    {
        ConnectTimeout = TimeSpan.FromSeconds(30),
    })
    .Build();
```

This API is also available for other transports (`LegacyHttpTransportOptions`, `NativeHttpTransportOptions`, and `UnixSocketTransportOptions`).

### Custom HTTP handler

Use `DockerClientBuilder.WithTransportOptions(...)` to choose among built-in transports, or instantiate `DockerClientBuilder<TTransportOptions>` with a custom `IDockerHandlerFactory<TTransportOptions>` implementation.
This allows you to define custom handlers for communicating with the Docker Engine API.

#### Example: List containers

```csharp
IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
    new ContainersListParameters
    {
        Limit = 10,
    });
```

#### Example: Create an image by pulling from Docker Registry

The code below pulls `fedora/memcached` image to your Docker instance using your Docker Hub account. You can
anonymously download the image as well by passing `null` instead of `AuthConfig` object:

```csharp
await client.Images.CreateImageAsync(
    new ImagesCreateParameters
    {
        FromImage = "fedora/memcached",
        Tag = "alpha",
    },
    new AuthConfig
    {
        Email = "test@example.com",
        Username = "test",
        Password = "pa$$w0rd"
    },
    new Progress<JSONMessage>());
```

#### Example: Create a container

The following code will create a new container of the previously fetched image:

```csharp
await client.Containers.CreateContainerAsync(
    new CreateContainerParameters
    {
        Image = "fedora/memcached",
        HostConfig = new HostConfig
        {
            DNS = new[] { "8.8.8.8", "8.8.4.4" }
        }
    });
```

#### Example: Start a container

The following code will start the created container:

```csharp
await client.Containers.StartContainerAsync(
    "39e3317fd258",
    new ContainerStartParameters());
```

#### Example: Stop a container

The following code will stop a running container.

**Note**: `WaitBeforeKillSeconds` field is of type `uint?` which means optional. This code will wait 30 seconds before
killing it. If you like to cancel the waiting, you can use the cancellation token parameter.

```csharp
bool hasStopped = await client.Containers.StopContainerAsync(
    "39e3317fd258",
    new ContainerStopParameters
    {
        WaitBeforeKillSeconds = 30
    },
    CancellationToken.None);
```

#### Example: Dealing with Stream responses

Some Docker Engine API endpoints are designed to return stream responses. For example
[Monitor events](https://docs.docker.com/reference/api/engine/version/v1.49/#tag/System/operation/SystemEvents)
continuously streams the status in a format like:

```json
{"status":"create","id":"dfdf82bd3881","from":"base:latest","time":1374067924}
{"status":"start","id":"dfdf82bd3881","from":"base:latest","time":1374067924}
{"status":"stop","id":"dfdf82bd3881","from":"base:latest","time":1374067966}
{"status":"destroy","id":"dfdf82bd3881","from":"base:latest","time":1374067970}
...
```

To obtain this stream you can use:

```csharp
Stream stream = await client.System.MonitorEventsAsync(
    new ContainerEventsParameters(),
    new Progress<Message>(),
    CancellationToken.None);
```

You can cancel streaming using the cancellation token. Or, if you wish to continuously stream, you can simply use `CancellationToken.None`.

#### Example: HTTPS authentication to Docker

If you are [running Docker with TLS (HTTPS)][docker-tls], you can authenticate to the Docker instance using the [**`Docker.DotNet.Enhanced.X509`**][Docker.DotNet.X509] package. You can get this package from NuGet or by running the following command in the "Package Manager Console":

```console
PM> Install-Package Docker.DotNet.Enhanced.X509
```

Once you add `Docker.DotNet.Enhanced.X509` to your project, use the `CertificateCredentials` type:

```csharp
var authProvider = new CertificateCredentials(new X509Certificate2("CertFile", "Password"));
var client = new DockerClientBuilder()
    .WithEndpoint(new Uri("https://ubuntu-docker.cloudapp.net:4243"))
    .WithAuthProvider(authProvider)
    .Build();
```

If you don't want to authenticate with a client certificate, omit `.WithAuthProvider(credentials)`.

The `CertFile` in the example above should be a PFX file (PKCS12 format), if you have PEM formatted certificates which Docker normally uses you can either convert it programmatically or use `openssl` tool to generate a PFX:

```console
openssl pkcs12 -export -inkey key.pem -in cert.pem -out key.pfx
```

(Here, your private key is `key.pem`, public key is `cert.pem` and output file is named `key.pfx`.) This will prompt a password for PFX file and then you can use this PFX file on Windows. If the certificate is self-signed, your application may reject the server certificate, in this case you might want to disable server certificate validation:

```csharp
var credentials = new CertificateCredentials(new X509Certificate2("CertFile", "Password"));
credentials.ServerCertificateValidationCallback = (o, c, ch, er) => true;
```

#### Example: HTTP authentication

If the Docker instance is secured with "Basic" HTTP authentication, you can use the [**`Docker.DotNet.Enhanced.BasicAuth`**][Docker.DotNet.BasicAuth] package. Get this package from NuGet or by running the following command in the "Package Manager Console":

```console
PM> Install-Package Docker.DotNet.Enhanced.BasicAuth
```

Once you added `Docker.DotNet.Enhanced.BasicAuth` to your project, use `BasicAuthCredentials` type:

```csharp
var authProvider = new BasicAuthCredentials("YOUR_USERNAME", "YOUR_PASSWORD");
var client = new DockerClientBuilder()
    .WithEndpoint(new Uri("tcp://ubuntu-docker.cloudapp.net:4243"))
    .WithAuthProvider(authProvider)
    .Build();
```

`BasicAuthCredentials` also accepts `SecureString` for username and password arguments.

#### Example: Specifying Remote API Version

By default this client does not specify version number to the API for the requests it makes.
However, if you would like to make use of versioning feature of Docker Remote API You can initialize the client like the following.

```csharp
var client = new DockerClientBuilder()
    .WithEndpoint(new Uri("http://ubuntu-docker.cloudapp.net:4243"))
    .WithApiVersion(new Version(1, 52))
    .Build();
```

### Error Handling

Here are typical exceptions thrown from the client library:

- **`DockerApiException`** is thrown when Docker Engine API responds with a non-success result. Subclasses:
    - **`DockerContainerNotFoundException`**
    - **`DockerImageNotFoundException`**
- **`TaskCanceledException`** is thrown from `System.Net.Http.HttpClient` library by design. It is not a friendly exception, but it indicates your request has timed out. (default request timeout is 100 seconds.)
    - Long-running methods (e.g. `WaitContainerAsync`, `StopContainerAsync`) and methods that return Stream (e.g. `CreateImageAsync`, `GetContainerLogsAsync`) have timeout value overridden with infinite timespan by this library.
- **`ArgumentNullException`** is thrown when one of the required parameters are missing/empty.
    - Consider reading the [Docker Remote API reference][docker-remote-api] and source code of the corresponding method you are going to use in from this library. This way you can easily find out which parameters are required and their format.

## License

Docker.DotNet is licensed under the [MIT](LICENSE) license.

---

- Copyright (c) .NET Foundation and Contributors
- Copyright (c) Andre Hofmeister

[docker-remote-api]: https://docs.docker.com/engine/reference/api/docker_remote_api/
[docker-tls]: https://docs.docker.com/engine/security/protect-access/
[nuget]: http://www.nuget.org
[nuget-gallery]: https://www.nuget.org/packages/Docker.DotNet.Enhanced/
[Docker.DotNet.X509]: https://www.nuget.org/packages/Docker.DotNet.Enhanced.X509/
[Docker.DotNet.BasicAuth]: https://www.nuget.org/packages/Docker.DotNet.Enhanced.BasicAuth/
