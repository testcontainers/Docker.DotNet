namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class IImageOperationsTests
{
    private readonly TestFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public IImageOperationsTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> GetDockerClientTypes() =>
        TestFixture.GetDockerClientTypes();

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task CreateImageAsync_TaskCancelled_ThrowsTaskCanceledException(TestClientsEnum clientType)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);

        var newTag = Guid.NewGuid().ToString();
        var newRepositoryName = Guid.NewGuid().ToString();

        await _testFixture.DockerClients[clientType].Images.TagImageAsync(
            $"{_testFixture.Repository}:{_testFixture.Tag}",
            new ImageTagParameters
            {
                RepositoryName = newRepositoryName,
                Tag = newTag,
            },
            cts.Token
        );

        var createImageTask = _testFixture.DockerClients[clientType].Images.CreateImageAsync(
            new ImagesCreateParameters
            {
                FromImage = $"{newRepositoryName}:{newTag}"
            },
            null,
            new Progress<JSONMessage>(message => _testOutputHelper.WriteLine(JsonSerializer.Instance.Serialize(message))),
            cts.Token);

        TimeSpan delay = TimeSpan.FromMilliseconds(5);
        cts.CancelAfter(delay);

        await Assert.ThrowsAsync<TaskCanceledException>(() => createImageTask);

        Assert.True(createImageTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public Task CreateImageAsync_ErrorResponse_ThrowsDockerApiException(TestClientsEnum clientType)
    {
        return Assert.ThrowsAsync<DockerApiException>(() => _testFixture.DockerClients[clientType].Images.CreateImageAsync(
            new ImagesCreateParameters
            {
                FromImage = "1.2.3.Apparently&this$is+not-a_valid%repository//name",
                Tag = "ancient-one"
            }, null, null));
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task DeleteImageAsync_RemovesImage(TestClientsEnum clientType)
    {
        var newImageTag = Guid.NewGuid().ToString();

        await _testFixture.DockerClients[clientType].Images.TagImageAsync(
            $"{_testFixture.Repository}:{_testFixture.Tag}",
            new ImageTagParameters
            {
                RepositoryName = _testFixture.Repository,
                Tag = newImageTag
            },
            _testFixture.Cts.Token
        );

        var inspectExistingImageResponse = await _testFixture.DockerClients[clientType].Images.InspectImageAsync(
            $"{_testFixture.Repository}:{newImageTag}",
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClients[clientType].Images.DeleteImageAsync(
            $"{_testFixture.Repository}:{newImageTag}",
            new ImageDeleteParameters(),
            _testFixture.Cts.Token
        );

        Task inspectDeletedImageTask = _testFixture.DockerClients[clientType].Images.InspectImageAsync(
            $"{_testFixture.Repository}:{newImageTag}",
            _testFixture.Cts.Token
        );

        Assert.NotNull(inspectExistingImageResponse);
        await Assert.ThrowsAsync<DockerImageNotFoundException>(() => inspectDeletedImageTask);
    }
}