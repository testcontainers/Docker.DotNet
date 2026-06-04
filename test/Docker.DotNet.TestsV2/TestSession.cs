namespace Docker.DotNet.TestsV2;

public static class TestSession
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, string>> Stages = new ConcurrentDictionary<string, Dictionary<string, string>>();

    public static readonly string TempDirectoryPath = Path.Combine(Path.GetTempPath(), "docker-dotnet", Guid.NewGuid().ToString("D"));

    static TestSession()
    {
        Directory.CreateDirectory(TempDirectoryPath);
    }
}