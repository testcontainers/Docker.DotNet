[![NuGet](https://img.shields.io/nuget/v/Docker.DotNet.Enhanced.svg)](https://www.nuget.org/packages/Docker.DotNet.Enhanced)

> [!IMPORTANT]
> Unfortunately, there has been no further development or bug fixes in [Docker.DotNet](https://github.com/dotnet/Docker.DotNet/tree/aacf3c26131f582ca8acc34084663a4b79e28d38) for some time now, and the repository appears inactive. Reaching the current maintainer is challenging. I understand that priorities shift, and maintaining an open-source project is time-consuming and exhausting. As maintainers, we often cannot dedicate the necessary time. Over the past months—and even years—I have frequently offered my help.
>
> Docker.DotNet is an important upstream dependency for Testcontainers for .NET and many other developers. It would be unfortunate for this valuable work not to continue. Therefore, I have decided to fork the repository to focus on essential fixes, improvements, and updates for the Docker Engine API. I am also accepting further contributions and remaining PRs from the upstream repository.

# .NET Client for Docker Remote API

This library allows you to interact with [Docker Remote API][docker-remote-api]  endpoints in your .NET applications.

It is fully asynchronous, designed to be non-blocking and object-oriented way to interact with your Docker daemon programmatically.

## Versioning

Version of this package uses [SemVer](https://semver.org/) format: `MAJOR.MINOR.PATCH`. `MINOR` segment indicates
the [Docker Remote API][docker-remote-api] version support. For instance `v2.124.0` of this library supports
[Docker Remote API][docker-remote-api] `v1.24`. This does not guarantee backwards compatibility as [Docker Remote API][docker-remote-api] does not guarantee that either.

`MAJOR` is reserved for major breaking changes we make to the library itself such as how
the calls are made or how authentication is made. `PATCH` is just for incremental bug fixes
or non-breaking feature additions.

## Installation

[![NuGet latest release](https://img.shields.io/nuget/v/Docker.DotNet.Enhanced.svg)](https://www.nuget.org/packages/Docker.DotNet.Enhanced)

You can add this library to your project using [NuGet][nuget].

**Package Manager Console**
Run the following command in the “Package Manager Console”:

> PM> Install-Package Docker.DotNet.Enhanced

**Visual Studio**
Right click to your project in Visual Studio, choose “Manage NuGet Packages” and search for ‘Docker.DotNet’ and click ‘Install’.
([see NuGet Gallery][nuget-gallery].)

**.NET Core Command Line Interface**
Run the following command from your favorite shell or terminal:

> dotnet add package Docker.DotNet.Enhanced

**Development Builds**

[![CI](https://github.com/testcontainers/Docker.DotNet/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/testcontainers/Docker.DotNet/actions/workflows/ci.yml)

## Usage

You can initialize the client like the following:

```csharp
using Docker.DotNet;
DockerClient client = new DockerClientConfiguration(
    new Uri("http://ubuntu-docker.cloudapp.net:4243"))
     .CreateClient();
```
or to connect to your local [Docker for Windows](https://docs.docker.com/docker-for-windows/) daemon using named pipes or your local [Docker for Mac](https://docs.docker.com/docker-for-mac/) daemon using Unix sockets:

```csharp
using Docker.DotNet;
DockerClient client = new DockerClientConfiguration()
     .CreateClient();
```

For a custom endpoint, you can also pass a named pipe or a Unix socket to the `DockerClientConfiguration` constructor. For example:

```csharp
// Default Docker Engine on Windows
using Docker.DotNet;
DockerClient client = new DockerClientConfiguration(
    new Uri("npipe://./pipe/docker_engine"))
     .CreateClient();
// Default Docker Engine on Linux
using Docker.DotNet;
DockerClient client = new DockerClientConfiguration(
    new Uri("unix:///var/run/docker.sock"))
     .CreateClient();
```

#### Example: List containers

```csharp
IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
	new ContainersListParameters(){
		Limit = 10,
    });
```

#### Example: Create an image by pulling from Docker Registry

The code below pulls `fedora/memcached` image to your Docker instance using your Docker Hub account. You can
anonymously download the image as well by passing `null` instead of AuthConfig object:

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

The following code will create a new container of the previously fetched image.

```csharp
await client.Containers.CreateContainerAsync(new CreateContainerParameters()
    {
        Image = "fedora/memcached",
        HostConfig = new HostConfig()
        {
            DNS = new[] { "8.8.8.8", "8.8.4.4" }
        }
    });
```

#### Example: Start a container

The following code will start the created container.

```csharp
await client.Containers.StartContainerAsync(
    "39e3317fd258",
    new ContainerStartParameters()
    );
```

#### Example: Stop a container

The following code will stop a running container.

*Note: `WaitBeforeKillSeconds` field is of type `uint?` which means optional. This code will wait 30 seconds before
killing it. If you like to cancel the waiting, you can use the CancellationToken parameter.*

```csharp
var stopped = await client.Containers.StopContainerAsync(
    "39e3317fd258",
    new ContainerStopParameters
    {
        WaitBeforeKillSeconds = 30
    },
    CancellationToken.None);
```

#### Example: Dealing with Stream responses

Some Docker API endpoints are designed to return stream responses. For example
[Monitoring Docker events](https://docs.docker.com/engine/reference/api/docker_remote_api_v1.24/#/monitor-docker-s-events)
continuously streams the status in a format like :

```json
{"status":"create","id":"dfdf82bd3881","from":"base:latest","time":1374067924}
{"status":"start","id":"dfdf82bd3881","from":"base:latest","time":1374067924}
{"status":"stop","id":"dfdf82bd3881","from":"base:latest","time":1374067966}
{"status":"destroy","id":"dfdf82bd3881","from":"base:latest","time":1374067970}
...
```

To obtain this stream you can use:

```csharp
CancellationTokenSource cancellation = new CancellationTokenSource();
Stream stream = await client.System.MonitorEventsAsync(new ContainerEventsParameters(), new Progress<JSONMessage>(), cancellation.Token);
// Initialize a StreamReader...
```

You can cancel streaming using the CancellationToken. On the other hand, if you wish to continuously stream, you can simply pass `CancellationToken.None`.

#### Example: HTTPS Authentication to Docker

If you are [running Docker with TLS (HTTPS)][docker-tls], you can authenticate to the Docker instance using the [**`Docker.DotNet.X509`**][Docker.DotNet.X509] package. You can get this package from NuGet or by running the following command in the “Package Manager Console”:

    PM> Install-Package Docker.DotNet.X509

Once you add `Docker.DotNet.X509` to your project, use `CertificateCredentials` type:

```csharp
var credentials = new CertificateCredentials (new X509Certificate2 ("CertFile", "Password"));
var config = new DockerClientConfiguration("http://ubuntu-docker.cloudapp.net:4243", credentials);
DockerClient client = config.CreateClient();
```

If you don't want to authenticate you can omit the `credentials` parameter, which defaults to an `AnonymousCredentials` instance.

The `CertFile` in the example above should be a .pfx file (PKCS12 format), if you have .pem formatted certificates which Docker normally uses you can either convert it programmatically or use `openssl` tool to generate a .pfx:

    openssl pkcs12 -export -inkey key.pem -in cert.pem -out key.pfx

(Here, your private key is key.pem, public key is cert.pem and output file is named key.pfx.) This will prompt a password for PFX file and then you can use this PFX file on Windows. If the certificate is self-signed, your application may reject the server certificate, in this case you might want to disable server certificate validation:
```c#
//
// There are two options to do this.
//

// You can do this globally for all certificates:
ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

// Or you can do this on a credential by credential basis:
var creds = new CertificateCredentials(...);
creds.ServerCertificateValidationCallback += (o, c, ch, er) => true;

```

#### Example: Basic HTTP Authentication to Docker

If the Docker instance is secured with Basic HTTP Authentication, you can use the [**`Docker.DotNet.BasicAuth`**][Docker.DotNet.BasicAuth] package. Get this package from NuGet or by running the following command in the “Package Manager Console”:

    PM> Install-Package Docker.DotNet.BasicAuth

Once you added `Docker.DotNet.BasicAuth` to your project, use `BasicAuthCredentials` type:

```csharp
var credentials = new BasicAuthCredentials ("YOUR_USERNAME", "YOUR_PASSWORD");
var config = new DockerClientConfiguration("tcp://ubuntu-docker.cloudapp.net:4243", credentials);
DockerClient client = config.CreateClient();
```

`BasicAuthCredentials` also accepts `SecureString` for username and password arguments.

#### Example: Specifying Remote API Version

By default this client does not specify version number to the API for the requests it makes. However, if you would like to make use of versioning feature of Docker Remote API You can initialize the client like the following.

```csharp
var config = new DockerClientConfiguration(...);
DockerClient client = config.CreateClient(new Version(1, 16));
```

### Error Handling

Here are typical exceptions thrown from the client library:

* **`DockerApiException`** is thrown when Docker API responds with a non-success result. Subclasses:
    * **``DockerContainerNotFoundException``**
    * **``DockerImageNotFoundException``**
* **`TaskCanceledException`** is thrown from `System.Net.Http.HttpClient` library by design. It is not a friendly exception, but it indicates your request has timed out. (default request timeout is 100 seconds.)
    * Long-running methods (e.g. `WaitContainerAsync`, `StopContainerAsync`) and methods that return Stream (e.g. `CreateImageAsync`, `GetContainerLogsAsync`) have timeout value overridden with infinite timespan by this library.
* **`ArgumentNullException`** is thrown when one of the required parameters are missing/empty.
    * Consider reading the [Docker Remote API reference][docker-remote-api] and source code of the corresponding method you are going to use in from this library. This way you can easily find out which parameters are required and their format.

## License

Docker.DotNet is licensed under the [MIT](LICENSE) license.

---------------
Copyright (c) .NET Foundation and Contributors
Copyright (c) Andre Hofmeister

[docker-remote-api]: https://docs.docker.com/engine/reference/api/docker_remote_api/
[docker-tls]: https://docs.docker.com/articles/https/
[nuget]: http://www.nuget.org
[nuget-gallery]: https://www.nuget.org/packages/Docker.DotNet.Enhanced/
[Docker.DotNet.X509]: https://www.nuget.org/packages/Docker.DotNet.Enhanced.X509/
[Docker.DotNet.BasicAuth]: https://www.nuget.org/packages/Docker.DotNet.Enhanced.BasicAuth/
