#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Info contains response of Engine API:
    /// GET &quot;/info&quot;
    /// </summary>
    public class SystemInfoResponse // (system.Info)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Containers")]
        public long Containers { get; set; } = default!;

        [JsonPropertyName("ContainersRunning")]
        public long ContainersRunning { get; set; } = default!;

        [JsonPropertyName("ContainersPaused")]
        public long ContainersPaused { get; set; } = default!;

        [JsonPropertyName("ContainersStopped")]
        public long ContainersStopped { get; set; } = default!;

        [JsonPropertyName("Images")]
        public long Images { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("DriverStatus")]
        public IList<string[]> DriverStatus { get; set; } = default!;

        /// <summary>
        /// SystemStatus is only propagated by the Swarm standalone API
        /// </summary>
        [JsonPropertyName("SystemStatus")]
        public IList<string[]>? SystemStatus { get; set; }

        [JsonPropertyName("Plugins")]
        public PluginsInfo Plugins { get; set; } = default!;

        [JsonPropertyName("MemoryLimit")]
        public bool MemoryLimit { get; set; } = default!;

        [JsonPropertyName("SwapLimit")]
        public bool SwapLimit { get; set; } = default!;

        [JsonPropertyName("CpuCfsPeriod")]
        public bool CPUCfsPeriod { get; set; } = default!;

        [JsonPropertyName("CpuCfsQuota")]
        public bool CPUCfsQuota { get; set; } = default!;

        [JsonPropertyName("CPUShares")]
        public bool CPUShares { get; set; } = default!;

        [JsonPropertyName("CPUSet")]
        public bool CPUSet { get; set; } = default!;

        [JsonPropertyName("PidsLimit")]
        public bool PidsLimit { get; set; } = default!;

        [JsonPropertyName("IPv4Forwarding")]
        public bool IPv4Forwarding { get; set; } = default!;

        [JsonPropertyName("Debug")]
        public bool Debug { get; set; } = default!;

        [JsonPropertyName("NFd")]
        public long NFd { get; set; } = default!;

        [JsonPropertyName("OomKillDisable")]
        public bool OomKillDisable { get; set; } = default!;

        [JsonPropertyName("NGoroutines")]
        public long NGoroutines { get; set; } = default!;

        [JsonPropertyName("SystemTime")]
        public string SystemTime { get; set; } = default!;

        [JsonPropertyName("LoggingDriver")]
        public string LoggingDriver { get; set; } = default!;

        [JsonPropertyName("CgroupDriver")]
        public string CgroupDriver { get; set; } = default!;

        [JsonPropertyName("CgroupVersion")]
        public string? CgroupVersion { get; set; }

        [JsonPropertyName("NEventsListener")]
        public long NEventsListener { get; set; } = default!;

        [JsonPropertyName("KernelVersion")]
        public string KernelVersion { get; set; } = default!;

        [JsonPropertyName("OperatingSystem")]
        public string OperatingSystem { get; set; } = default!;

        [JsonPropertyName("OSVersion")]
        public string OSVersion { get; set; } = default!;

        [JsonPropertyName("OSType")]
        public string OSType { get; set; } = default!;

        [JsonPropertyName("Architecture")]
        public string Architecture { get; set; } = default!;

        [JsonPropertyName("IndexServerAddress")]
        public string IndexServerAddress { get; set; } = default!;

        [JsonPropertyName("RegistryConfig")]
        public ServiceConfig? RegistryConfig { get; set; }

        [JsonPropertyName("NCPU")]
        public long NCPU { get; set; } = default!;

        [JsonPropertyName("MemTotal")]
        public long MemTotal { get; set; } = default!;

        [JsonPropertyName("GenericResources")]
        public IList<GenericResource> GenericResources { get; set; } = default!;

        [JsonPropertyName("DockerRootDir")]
        public string DockerRootDir { get; set; } = default!;

        [JsonPropertyName("HttpProxy")]
        public string HTTPProxy { get; set; } = default!;

        [JsonPropertyName("HttpsProxy")]
        public string HTTPSProxy { get; set; } = default!;

        [JsonPropertyName("NoProxy")]
        public string NoProxy { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IList<string> Labels { get; set; } = default!;

        [JsonPropertyName("ExperimentalBuild")]
        public bool ExperimentalBuild { get; set; } = default!;

        [JsonPropertyName("ServerVersion")]
        public string ServerVersion { get; set; } = default!;

        [JsonPropertyName("Runtimes")]
        public IDictionary<string, RuntimeWithStatus> Runtimes { get; set; } = default!;

        [JsonPropertyName("DefaultRuntime")]
        public string DefaultRuntime { get; set; } = default!;

        [JsonPropertyName("Swarm")]
        public Info Swarm { get; set; } = default!;

        /// <summary>
        /// LiveRestoreEnabled determines whether containers should be kept
        /// running when the daemon is shutdown or upon daemon start if
        /// running containers are detected
        /// </summary>
        [JsonPropertyName("LiveRestoreEnabled")]
        public bool LiveRestoreEnabled { get; set; } = default!;

        [JsonPropertyName("Isolation")]
        public string Isolation { get; set; } = default!;

        [JsonPropertyName("InitBinary")]
        public string InitBinary { get; set; } = default!;

        [JsonPropertyName("ContainerdCommit")]
        public Commit ContainerdCommit { get; set; } = default!;

        [JsonPropertyName("RuncCommit")]
        public Commit RuncCommit { get; set; } = default!;

        [JsonPropertyName("InitCommit")]
        public Commit InitCommit { get; set; } = default!;

        [JsonPropertyName("SecurityOptions")]
        public IList<string> SecurityOptions { get; set; } = default!;

        [JsonPropertyName("ProductLicense")]
        public string? ProductLicense { get; set; }

        [JsonPropertyName("DefaultAddressPools")]
        public IList<NetworkAddressPool>? DefaultAddressPools { get; set; }

        [JsonPropertyName("FirewallBackend")]
        public FirewallInfo? FirewallBackend { get; set; }

        [JsonPropertyName("CDISpecDirs")]
        public IList<string> CDISpecDirs { get; set; } = default!;

        [JsonPropertyName("DiscoveredDevices")]
        public IList<DeviceInfo>? DiscoveredDevices { get; set; }

        [JsonPropertyName("NRI")]
        public NRIInfo? NRI { get; set; }

        [JsonPropertyName("Containerd")]
        public ContainerdInfo? Containerd { get; set; }

        /// <summary>
        /// Warnings contains a slice of warnings that occurred  while collecting
        /// system information. These warnings are intended to be informational
        /// messages for the user, and are not intended to be parsed / used for
        /// other purposes, as they do not have a fixed format.
        /// </summary>
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}
