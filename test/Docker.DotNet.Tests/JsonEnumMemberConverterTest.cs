namespace Docker.DotNet.Tests;

public sealed class JsonEnumMemberConverterTests
{
    [Theory]
    [ClassData(typeof(RestartPolicyKindTestData))]
    public void JsonSerialization_ShouldSerializeAndDeserializeCorrectly(RestartPolicyKind restartPolicyKind)
    {
        // Given
        var parameters = new CreateContainerParameters
        {
            HostConfig = new HostConfig
            {
                RestartPolicy = new RestartPolicy
                {
                    Name = restartPolicyKind
                }
            }
        };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(parameters);
        var deserializedParameters = JsonSerializer.Instance.Deserialize<CreateContainerParameters>(Encoding.UTF8.GetBytes(jsonString));

        // Then
        Assert.Equal(restartPolicyKind, deserializedParameters.HostConfig.RestartPolicy.Name);
    }

    private sealed class RestartPolicyKindTestData : TheoryData<RestartPolicyKind>
    {
        public RestartPolicyKindTestData()
        {
            Add(RestartPolicyKind.Undefined);
            Add(RestartPolicyKind.No);
            Add(RestartPolicyKind.Always);
            Add(RestartPolicyKind.OnFailure);
            Add(RestartPolicyKind.UnlessStopped);
        }
    }
}