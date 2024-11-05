using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Docker.DotNet
{
    internal class ConfigOperations : IConfigOperations
    {
        private readonly DockerClient _client;

        internal ConfigOperations(DockerClient client)
        {
            _client = client;
        }

        async Task<IList<SwarmConfig>> IConfigOperations.ListConfigsAsync(CancellationToken cancellationToken)
        {
            return await _client.MakeRequestAsync<IList<SwarmConfig>>(_client.NoErrorHandlers, HttpMethod.Get, "configs", cancellationToken).ConfigureAwait(false);
        }

        async Task<SwarmCreateConfigResponse> IConfigOperations.CreateConfigAsync(SwarmCreateConfigParameters body, CancellationToken cancellationToken)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var data = new JsonRequestContent<SwarmConfigSpec>(body.Config, _client.JsonSerializer);
            return await _client.MakeRequestAsync<SwarmCreateConfigResponse>(_client.NoErrorHandlers, HttpMethod.Post, "configs/create", null, data, cancellationToken).ConfigureAwait(false);
        }

        async Task<SwarmConfig> IConfigOperations.InspectConfigAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.MakeRequestAsync<SwarmConfig>(_client.NoErrorHandlers, HttpMethod.Get, $"configs/{id}", cancellationToken).ConfigureAwait(false);
        }

        Task IConfigOperations.RemoveConfigAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Delete, $"configs/{id}", cancellationToken);
        }
    }
}