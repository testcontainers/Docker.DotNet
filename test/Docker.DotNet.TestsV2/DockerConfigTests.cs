namespace Docker.DotNet.TestsV2;

public static class DockerConfigTests
{
    public sealed class DockerContextConfigurationTests
    {
        [Fact]
        public void ReturnsActiveEndpointWhenDockerContextIsUnset()
        {
            IDockerCliSettings settings = new TestDockerCliSettings();

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(DockerCli.GetCurrentEndpoint(), currentEndpoint);
        }

        [Fact]
        public void ReturnsActiveEndpointWhenDockerContextIsEmpty()
        {
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerContext = string.Empty
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(DockerCli.GetCurrentEndpoint(), currentEndpoint);
        }

        [Fact]
        public void ReturnsDefaultEndpointWhenDockerContextIsDefault()
        {
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerContext = "default"
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(DockerCli.GetCurrentEndpoint("default"), currentEndpoint);
        }

        [Fact]
        public void ReturnsDefaultEndpointWhenNamedContextIsDefault()
        {
            var dockerConfig = new DockerConfig(new TestDockerCliSettings());

            var currentEndpoint = dockerConfig.GetEndpoint("default");

            Assert.Equal(DockerCli.GetCurrentEndpoint("default"), currentEndpoint);
        }

        [Fact]
        public void ReturnsConfiguredEndpointWhenDockerContextIsCustomFromPropertiesFile()
        {
            using var context = new ConfigMetaFile("custom", new Uri("tcp://127.0.0.1:2375/"));

            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerConfig = context.DockerConfigDirectoryPath,
                DockerContext = "custom"
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(new Uri("tcp://127.0.0.1:2375/"), currentEndpoint);
        }

        [Fact]
        public void ReturnsConfiguredEndpointWhenDockerContextIsCustomFromConfigFile()
        {
            using var context = new ConfigMetaFile("custom", new Uri("tcp://127.0.0.1:2375/"));

            // This test reads the current context JSON node from the Docker config file.
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerConfig = context.DockerConfigDirectoryPath
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(new Uri("tcp://127.0.0.1:2375/"), currentEndpoint);
        }

        [Fact]
        public void ReturnsConfiguredEndpointWhenContextNameContainsUnicodeCharacters()
        {
            using var context = new ConfigMetaFile("münchen", new Uri("tcp://127.0.0.1:2375/"));

            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerConfig = context.DockerConfigDirectoryPath,
                DockerContext = "münchen"
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(new Uri("tcp://127.0.0.1:2375/"), currentEndpoint);
        }

        [Fact]
        public void ThrowsWhenDockerContextNotFound()
        {
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerContext = "missing"
            };

            var dockerConfig = new DockerConfig(settings);

            var exception = Assert.Throws<DockerConfigurationException>(dockerConfig.GetCurrentEndpoint);

            Assert.Equal("The Docker context 'missing' does not exist.", exception.Message);
            Assert.IsType<DirectoryNotFoundException>(exception.InnerException);
        }

        [Fact]
        public void ThrowsWhenDockerHostNotFound()
        {
            using var context = new ConfigMetaFile("custom", null);

            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerConfig = context.DockerConfigDirectoryPath,
                DockerContext = "custom"
            };

            var dockerConfig = new DockerConfig(settings);

            var exception = Assert.Throws<DockerConfigurationException>(dockerConfig.GetCurrentEndpoint);

            Assert.StartsWith("The Docker host is null or empty in ", exception.Message);
            Assert.Contains(context.DockerConfigDirectoryPath, exception.Message);
            Assert.EndsWith(" (JSONPath: Endpoints.docker.Host).", exception.Message);
            Assert.Null(exception.InnerException);
        }
    }

    public sealed class DockerHostConfigurationTests
    {
        [Fact]
        public void ReturnsActiveEndpointWhenDockerHostIsEmpty()
        {
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerHost = string.Empty
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(DockerCli.GetCurrentEndpoint(), currentEndpoint);
        }

        [Fact]
        public void ReturnsConfiguredEndpointWhenDockerHostIsSet()
        {
            using var context = new ConfigMetaFile("custom", null);

            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerConfig = context.DockerConfigDirectoryPath,
                DockerHost = "tcp://127.0.0.1:2375/"
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(new Uri("tcp://127.0.0.1:2375/"), currentEndpoint);
        }

        [Fact]
        public void ReturnsSecureEndpointWhenDockerHostIsSetAndTlsVerifyIsEnabled()
        {
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerHost = "tcp://127.0.0.1:2376/",
                DockerTlsVerify = "1"
            };

            var dockerConfig = new DockerConfig(settings);

            var currentEndpoint = dockerConfig.GetCurrentEndpoint();

            Assert.Equal(new Uri("https://127.0.0.1:2376/"), currentEndpoint);
        }

        [Fact]
        public void ThrowsWhenDockerHostIsInvalid()
        {
            IDockerCliSettings settings = new TestDockerCliSettings
            {
                DockerHost = "not-a-valid-uri"
            };

            var dockerConfig = new DockerConfig(settings);

            var exception = Assert.Throws<DockerConfigurationException>(dockerConfig.GetCurrentEndpoint);

            Assert.Equal("The Docker host 'not-a-valid-uri' is invalid.", exception.Message);
        }
    }

    private sealed class TestDockerCliSettings : IDockerCliSettings
    {
        public string DockerConfig { get; init; }

        public string DockerHost { get; init; }

        public string DockerTlsVerify { get; init; }

        public string DockerContext { get; init; }
    }
}