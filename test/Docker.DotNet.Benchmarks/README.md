# Docker.DotNet Benchmarks

This benchmark project compares the current `main` source code against a released NuGet package by running the same benchmark class twice.

## Run

From the repository root, run `main` first:

```bash
dotnet run -c Release --project test/Docker.DotNet.Benchmarks/Docker.DotNet.Benchmarks.csproj -- --filter '*DockerDaemonRoundtripBenchmarks*'
```

Then run the released NuGet package implementation:

```bash
dotnet run -c Release --project test/Docker.DotNet.Benchmarks/Docker.DotNet.Benchmarks.csproj -p:UseReleasedPackage=true -- --filter '*DockerDaemonRoundtripBenchmarks*'
```

To compare against a different release tag/package version:

```bash
dotnet run -c Release --project test/Docker.DotNet.Benchmarks/Docker.DotNet.Benchmarks.csproj -p:UseReleasedPackage=true -p:DockerDotNetReleaseVersion=3.132.0 -- --filter '*DockerDaemonRoundtripBenchmarks*'
```
