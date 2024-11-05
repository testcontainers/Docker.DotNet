using Docker.DotNet.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.DotNet
{
    internal class VolumeOperations : IVolumeOperations
    {
        private readonly DockerClient _client;

        internal VolumeOperations(DockerClient client)
        {
            _client = client;
        }

        async Task<VolumesListResponse> IVolumeOperations.ListAsync(CancellationToken cancellationToken)
        {
            return await _client.MakeRequestAsync<VolumesListResponse>(_client.NoErrorHandlers, HttpMethod.Get, "volumes", cancellationToken).ConfigureAwait(false);
        }

        async Task<VolumesListResponse> IVolumeOperations.ListAsync(VolumesListParameters parameters, CancellationToken cancellationToken)
        {
            var queryParameters = parameters == null ? null : new QueryString<VolumesListParameters>(parameters);
            return await _client.MakeRequestAsync<VolumesListResponse>(_client.NoErrorHandlers, HttpMethod.Get, "volumes", queryParameters, null, cancellationToken).ConfigureAwait(false);
        }

        async Task<VolumeResponse> IVolumeOperations.CreateAsync(VolumesCreateParameters parameters, CancellationToken cancellationToken)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var data = new JsonRequestContent<VolumesCreateParameters>(parameters, _client.JsonSerializer);
            return await _client.MakeRequestAsync<VolumeResponse>(_client.NoErrorHandlers, HttpMethod.Post, "volumes/create", null, data, cancellationToken);
        }

        async Task<VolumeResponse> IVolumeOperations.InspectAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await _client.MakeRequestAsync<VolumeResponse>(_client.NoErrorHandlers, HttpMethod.Get, $"volumes/{name}", cancellationToken).ConfigureAwait(false);
        }

        Task IVolumeOperations.RemoveAsync(string name, bool? force, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Delete, $"volumes/{name}", cancellationToken);
        }

        async Task<VolumesPruneResponse> IVolumeOperations.PruneAsync(VolumesPruneParameters parameters, CancellationToken cancellationToken)
        {
            var queryParameters = parameters == null ? null : new QueryString<VolumesPruneParameters>(parameters);
            return await _client.MakeRequestAsync<VolumesPruneResponse>(_client.NoErrorHandlers, HttpMethod.Post, "volumes/prune", queryParameters, cancellationToken);
        }
    }
}