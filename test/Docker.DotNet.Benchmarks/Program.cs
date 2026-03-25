Type[] benchmarks = [typeof(DockerDaemonRoundtripBenchmarks)];
BenchmarkSwitcher.FromTypes(benchmarks).Run(args);