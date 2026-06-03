namespace Docker.DotNet.TestsV2;

internal sealed class ConfigMetaFile : IDisposable
{
    private const string ConfigFileJson = "{{\"currentContext\":\"{0}\"}}";

    private const string MetaFileJson = "{{\"Name\":\"{0}\",\"Metadata\":{{}},\"Endpoints\":{{\"docker\":{{\"Host\":\"{1}\",\"SkipTLSVerify\":false}}}}}}";

    public ConfigMetaFile(string context, Uri endpoint, [CallerMemberName] string caller = "")
    {
        DockerConfigDirectoryPath = Path.Combine(TestSession.TempDirectoryPath, caller);
        var dockerContextHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(context))).ToLowerInvariant();
        var dockerContextMetaDirectoryPath = Path.Combine(DockerConfigDirectoryPath, "contexts", "meta", dockerContextHash);
        _ = Directory.CreateDirectory(dockerContextMetaDirectoryPath);
        File.WriteAllText(Path.Combine(DockerConfigDirectoryPath, "config.json"), string.Format(ConfigFileJson, context));
        File.WriteAllText(Path.Combine(dockerContextMetaDirectoryPath, "meta.json"), endpoint == null ? "{}" : string.Format(MetaFileJson, context, endpoint.AbsoluteUri));
    }

    public string DockerConfigDirectoryPath { get; }

    public void Dispose()
    {
        Directory.Delete(DockerConfigDirectoryPath, true);
    }
}