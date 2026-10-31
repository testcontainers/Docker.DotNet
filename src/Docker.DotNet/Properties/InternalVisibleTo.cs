// Docker.DotNet.Handler.Abstractions exposes its own copy of StrongNamePublicKeys
// through InternalsVisibleTo, which conflicts with the copy compiled in here.
#pragma warning disable CS0436
[assembly: InternalsVisibleTo("Docker.DotNet.Tests" + StrongNamePublicKeys.DockerDotNetPublicKey)]
#pragma warning restore CS0436