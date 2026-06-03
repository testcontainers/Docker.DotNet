namespace Docker.DotNet;

[JsonSerializable(typeof(DockerConfig.DockerContextMeta))]
internal partial class SourceGenerationContext : JsonSerializerContext;