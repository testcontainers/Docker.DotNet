namespace Docker.DotNet;

internal class PluginOperations : IPluginOperations
{
    private const string TarContentType = "application/x-tar";

    private static readonly ApiResponseErrorHandlingDelegate NoSuchPluginHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerPluginNotFoundException(statusCode, responseBody);
        }
    };

    private readonly DockerClient _client;

    internal PluginOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<IList<Plugin>> ListPluginsAsync(PluginListParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<PluginListParameters>(parameters);

        return await _client.MakeRequestAsync<Plugin[]>(_client.NoErrorHandlers, HttpMethod.Get, "plugins", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IList<PluginPrivilege>> GetPrivilegesAsync(PluginGetPrivilegeParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<PluginGetPrivilegeParameters>(parameters);

        return await _client.MakeRequestAsync<PluginPrivilege[]>(_client.NoErrorHandlers, HttpMethod.Get, "plugins/privileges", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task InstallPluginAsync(PluginInstallParameters parameters, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (parameters.Privileges == null)
        {
            throw new ArgumentNullException(nameof(parameters.Privileges));
        }

        var queryParameters = new QueryString<PluginInstallParameters>(parameters);

        var data = new JsonRequestContent<IList<PluginPrivilege>>(parameters.Privileges, DockerClient.JsonSerializer);

        return StreamUtil.MonitorStreamForMessagesAsync(
            _client.MakeRequestForStreamAsync(_client.NoErrorHandlers, HttpMethod.Post, $"plugins/pull", queryParameters, data, null, cancellationToken),
            progress,
            cancellationToken);
    }

    public async Task<Plugin> InspectPluginAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return await _client.MakeRequestAsync<Plugin>([NoSuchPluginHandler], HttpMethod.Get, $"plugins/{name}/json", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RemovePluginAsync(string name, PluginRemoveParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var queryParameters = parameters == null ? null : new QueryString<PluginRemoveParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchPluginHandler], HttpMethod.Delete, $"plugins/{name}", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task EnablePluginAsync(string name, PluginEnableParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var queryParameters = parameters == null ? null : new QueryString<PluginEnableParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchPluginHandler], HttpMethod.Post, $"plugins/{name}/enable", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task DisablePluginAsync(string name, PluginDisableParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var queryParameters = parameters == null ? null : new QueryString<PluginDisableParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchPluginHandler], HttpMethod.Post, $"plugins/{name}/disable", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpgradePluginAsync(string name, PluginUpgradeParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (parameters.Privileges == null)
        {
            throw new ArgumentNullException(nameof(parameters.Privileges));
        }

        var queryParameters = new QueryString<PluginUpgradeParameters>(parameters);

        var data = new JsonRequestContent<IList<PluginPrivilege>>(parameters.Privileges, DockerClient.JsonSerializer);

        await _client.MakeRequestAsync([NoSuchPluginHandler], HttpMethod.Post, $"plugins/{name}/upgrade", queryParameters, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CreatePluginAsync(PluginCreateParameters parameters, Stream plugin, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (plugin == null)
        {
            throw new ArgumentNullException(nameof(plugin));
        }

        var queryParameters = new QueryString<PluginCreateParameters>(parameters);

        var data = new BinaryRequestContent(plugin, TarContentType);

        await _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Post, $"plugins/create", queryParameters, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task PushPluginAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        await _client.MakeRequestAsync([NoSuchPluginHandler], HttpMethod.Post, $"plugins/{name}/push", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task ConfigurePluginAsync(string name, PluginConfigureParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (parameters.Args == null)
        {
            throw new ArgumentNullException(nameof(parameters.Args));
        }

        var data = new JsonRequestContent<IList<string>>(parameters.Args, DockerClient.JsonSerializer);

        await _client.MakeRequestAsync([NoSuchPluginHandler], HttpMethod.Post, $"plugins/{name}/set", null, data, cancellationToken)
            .ConfigureAwait(false);
    }
}