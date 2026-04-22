namespace Docker.DotNet;

public interface IVolumeOperations
{
    /// <summary>
    /// List volumes
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 500 - Server error.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<VolumesListResponse> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// List volumes
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 500 - Server error.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<VolumesListResponse> ListAsync(VolumesListParameters? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a volume.
    /// </summary>
    /// <remarks>
    /// 201 - The volume was created successfully.
    /// 500 - Server error.
    /// </remarks>
    /// <param name="parameters">Volume parameters to create.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<VolumeResponse> CreateAsync(VolumesCreateParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inspect a volume.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 404 - No such volume.
    /// 500 - Server error.
    /// </remarks>
    /// <param name="name">Volume name or ID.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<VolumeResponse> InspectAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a volume.
    ///
    /// Instruct the driver to remove the volume.
    /// </summary>
    /// <remarks>
    /// 204 - The volume was removed.
    /// 404 - No such volume or volume driver.
    /// 409 - Volume is in use and cannot be removed.
    /// 500 - Server error.
    /// </remarks>
    /// <param name="name">Volume name or ID.</param>
    /// <param name="force">Force the removal of the volume.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task RemoveAsync(string name, bool? force = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete unused volumes.
    /// </summary>
    /// <remarks>
    /// HTTP POST /volumes/prune
    ///
    /// 200 - No error.
    /// 500 - Server error.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<VolumesPruneResponse> PruneAsync(VolumesPruneParameters? parameters = null, CancellationToken cancellationToken = default);
}