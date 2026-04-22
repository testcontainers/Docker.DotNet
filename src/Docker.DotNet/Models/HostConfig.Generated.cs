#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// HostConfig the non-portable Config structure of a container.
    /// Here, &quot;non-portable&quot; means &quot;dependent of the host we are running on&quot;.
    /// Portable information *should* appear in Config.
    /// </summary>
    public class HostConfig // (container.HostConfig)
    {
        public HostConfig()
        {
        }

        public HostConfig(Resources Resources)
        {
            if (Resources != null)
            {
                this.CPUShares = Resources.CPUShares;
                this.Memory = Resources.Memory;
                this.NanoCPUs = Resources.NanoCPUs;
                this.CgroupParent = Resources.CgroupParent;
                this.BlkioWeight = Resources.BlkioWeight;
                this.BlkioWeightDevice = Resources.BlkioWeightDevice;
                this.BlkioDeviceReadBps = Resources.BlkioDeviceReadBps;
                this.BlkioDeviceWriteBps = Resources.BlkioDeviceWriteBps;
                this.BlkioDeviceReadIOps = Resources.BlkioDeviceReadIOps;
                this.BlkioDeviceWriteIOps = Resources.BlkioDeviceWriteIOps;
                this.CPUPeriod = Resources.CPUPeriod;
                this.CPUQuota = Resources.CPUQuota;
                this.CPURealtimePeriod = Resources.CPURealtimePeriod;
                this.CPURealtimeRuntime = Resources.CPURealtimeRuntime;
                this.CpusetCpus = Resources.CpusetCpus;
                this.CpusetMems = Resources.CpusetMems;
                this.Devices = Resources.Devices;
                this.DeviceCgroupRules = Resources.DeviceCgroupRules;
                this.DeviceRequests = Resources.DeviceRequests;
                this.MemoryReservation = Resources.MemoryReservation;
                this.MemorySwap = Resources.MemorySwap;
                this.MemorySwappiness = Resources.MemorySwappiness;
                this.OomKillDisable = Resources.OomKillDisable;
                this.PidsLimit = Resources.PidsLimit;
                this.Ulimits = Resources.Ulimits;
                this.CPUCount = Resources.CPUCount;
                this.CPUPercent = Resources.CPUPercent;
                this.IOMaximumIOps = Resources.IOMaximumIOps;
                this.IOMaximumBandwidth = Resources.IOMaximumBandwidth;
            }
        }

        /// <summary>
        /// Applicable to all platforms
        /// </summary>
        [JsonPropertyName("Binds")]
        public IList<string> Binds { get; set; } = default!;

        /// <summary>
        /// File (path) where the containerId is written
        /// </summary>
        [JsonPropertyName("ContainerIDFile")]
        public string ContainerIDFile { get; set; } = default!;

        /// <summary>
        /// Configuration of the logs for this container
        /// </summary>
        [JsonPropertyName("LogConfig")]
        public LogConfig LogConfig { get; set; } = default!;

        /// <summary>
        /// Network mode to use for the container
        /// </summary>
        [JsonPropertyName("NetworkMode")]
        public string NetworkMode { get; set; } = default!;

        /// <summary>
        /// Port mapping between the exposed port (container) and the host
        /// </summary>
        [JsonPropertyName("PortBindings")]
        public IDictionary<string, IList<PortBinding>> PortBindings { get; set; } = default!;

        /// <summary>
        /// Restart policy to be used for the container
        /// </summary>
        [JsonPropertyName("RestartPolicy")]
        public RestartPolicy RestartPolicy { get; set; } = default!;

        /// <summary>
        /// Automatically remove container when it exits
        /// </summary>
        [JsonPropertyName("AutoRemove")]
        public bool AutoRemove { get; set; } = default!;

        /// <summary>
        /// Name of the volume driver used to mount volumes
        /// </summary>
        [JsonPropertyName("VolumeDriver")]
        public string VolumeDriver { get; set; } = default!;

        /// <summary>
        /// List of volumes to take from other container
        /// </summary>
        [JsonPropertyName("VolumesFrom")]
        public IList<string> VolumesFrom { get; set; } = default!;

        /// <summary>
        /// Initial console size (height,width)
        /// </summary>
        [JsonPropertyName("ConsoleSize")]
        [JsonConverter(typeof(JsonConsoleSizeConverter))]
        public ConsoleSize ConsoleSize { get; set; } = default!;

        /// <summary>
        /// Arbitrary non-identifying metadata attached to container and provided to the runtime
        /// </summary>
        [JsonPropertyName("Annotations")]
        public IDictionary<string, string>? Annotations { get; set; }

        /// <summary>
        /// Applicable to UNIX platforms
        /// </summary>
        [JsonPropertyName("CapAdd")]
        public IList<string> CapAdd { get; set; } = default!;

        /// <summary>
        /// List of kernel capabilities to remove from the container
        /// </summary>
        [JsonPropertyName("CapDrop")]
        public IList<string> CapDrop { get; set; } = default!;

        /// <summary>
        /// Cgroup namespace mode to use for the container
        /// </summary>
        [JsonPropertyName("CgroupnsMode")]
        public string CgroupnsMode { get; set; } = default!;

        /// <summary>
        /// List of DNS server to lookup
        /// </summary>
        [JsonPropertyName("Dns")]
        public IList<string> DNS { get; set; } = default!;

        /// <summary>
        /// List of DNSOption to look for
        /// </summary>
        [JsonPropertyName("DnsOptions")]
        public IList<string> DNSOptions { get; set; } = default!;

        /// <summary>
        /// List of DNSSearch to look for
        /// </summary>
        [JsonPropertyName("DnsSearch")]
        public IList<string> DNSSearch { get; set; } = default!;

        /// <summary>
        /// List of extra hosts
        /// </summary>
        [JsonPropertyName("ExtraHosts")]
        public IList<string> ExtraHosts { get; set; } = default!;

        /// <summary>
        /// List of additional groups that the container process will run as
        /// </summary>
        [JsonPropertyName("GroupAdd")]
        public IList<string> GroupAdd { get; set; } = default!;

        /// <summary>
        /// IPC namespace to use for the container
        /// </summary>
        [JsonPropertyName("IpcMode")]
        public string IpcMode { get; set; } = default!;

        /// <summary>
        /// Cgroup to use for the container
        /// </summary>
        [JsonPropertyName("Cgroup")]
        public string Cgroup { get; set; } = default!;

        /// <summary>
        /// List of links (in the name:alias form)
        /// </summary>
        [JsonPropertyName("Links")]
        public IList<string> Links { get; set; } = default!;

        /// <summary>
        /// Container preference for OOM-killing
        /// </summary>
        [JsonPropertyName("OomScoreAdj")]
        public long OomScoreAdj { get; set; } = default!;

        /// <summary>
        /// PID namespace to use for the container
        /// </summary>
        [JsonPropertyName("PidMode")]
        public string PidMode { get; set; } = default!;

        /// <summary>
        /// Is the container in privileged mode
        /// </summary>
        [JsonPropertyName("Privileged")]
        public bool Privileged { get; set; } = default!;

        /// <summary>
        /// Should docker publish all exposed port for the container
        /// </summary>
        [JsonPropertyName("PublishAllPorts")]
        public bool PublishAllPorts { get; set; } = default!;

        /// <summary>
        /// Is the container root filesystem in read-only
        /// </summary>
        [JsonPropertyName("ReadonlyRootfs")]
        public bool ReadonlyRootfs { get; set; } = default!;

        /// <summary>
        /// List of string values to customize labels for MLS systems, such as SELinux.
        /// </summary>
        [JsonPropertyName("SecurityOpt")]
        public IList<string> SecurityOpt { get; set; } = default!;

        /// <summary>
        /// Storage driver options per container.
        /// </summary>
        [JsonPropertyName("StorageOpt")]
        public IDictionary<string, string>? StorageOpt { get; set; }

        /// <summary>
        /// List of tmpfs (mounts) used for the container
        /// </summary>
        [JsonPropertyName("Tmpfs")]
        public IDictionary<string, string>? Tmpfs { get; set; }

        /// <summary>
        /// UTS namespace to use for the container
        /// </summary>
        [JsonPropertyName("UTSMode")]
        public string UTSMode { get; set; } = default!;

        /// <summary>
        /// The user namespace to use for the container
        /// </summary>
        [JsonPropertyName("UsernsMode")]
        public string UsernsMode { get; set; } = default!;

        /// <summary>
        /// Total shm memory usage
        /// </summary>
        [JsonPropertyName("ShmSize")]
        public long ShmSize { get; set; } = default!;

        /// <summary>
        /// List of Namespaced sysctls used for the container
        /// </summary>
        [JsonPropertyName("Sysctls")]
        public IDictionary<string, string>? Sysctls { get; set; }

        /// <summary>
        /// Runtime to use with this container
        /// </summary>
        [JsonPropertyName("Runtime")]
        public string? Runtime { get; set; }

        /// <summary>
        /// Applicable to Windows
        /// </summary>
        [JsonPropertyName("Isolation")]
        public string Isolation { get; set; } = default!;

        /// <summary>
        /// Applicable to all platforms
        /// </summary>
        [JsonPropertyName("CpuShares")]
        public long CPUShares { get; set; } = default!;

        /// <summary>
        /// Memory limit (in bytes)
        /// </summary>
        [JsonPropertyName("Memory")]
        public long Memory { get; set; } = default!;

        /// <summary>
        /// CPU quota in units of 10&lt;sup&gt;-9&lt;/sup&gt; CPUs.
        /// </summary>
        [JsonPropertyName("NanoCpus")]
        public long NanoCPUs { get; set; } = default!;

        /// <summary>
        /// Applicable to UNIX platforms
        /// </summary>
        [JsonPropertyName("CgroupParent")]
        public string CgroupParent { get; set; } = default!;

        /// <summary>
        /// Block IO weight (relative weight vs. other containers)
        /// </summary>
        [JsonPropertyName("BlkioWeight")]
        public ushort BlkioWeight { get; set; } = default!;

        [JsonPropertyName("BlkioWeightDevice")]
        public IList<WeightDevice> BlkioWeightDevice { get; set; } = default!;

        [JsonPropertyName("BlkioDeviceReadBps")]
        public IList<ThrottleDevice> BlkioDeviceReadBps { get; set; } = default!;

        [JsonPropertyName("BlkioDeviceWriteBps")]
        public IList<ThrottleDevice> BlkioDeviceWriteBps { get; set; } = default!;

        [JsonPropertyName("BlkioDeviceReadIOps")]
        public IList<ThrottleDevice> BlkioDeviceReadIOps { get; set; } = default!;

        [JsonPropertyName("BlkioDeviceWriteIOps")]
        public IList<ThrottleDevice> BlkioDeviceWriteIOps { get; set; } = default!;

        /// <summary>
        /// CPU CFS (Completely Fair Scheduler) period
        /// </summary>
        [JsonPropertyName("CpuPeriod")]
        public long CPUPeriod { get; set; } = default!;

        /// <summary>
        /// CPU CFS (Completely Fair Scheduler) quota
        /// </summary>
        [JsonPropertyName("CpuQuota")]
        public long CPUQuota { get; set; } = default!;

        /// <summary>
        /// CPU real-time period
        /// </summary>
        [JsonPropertyName("CpuRealtimePeriod")]
        public long CPURealtimePeriod { get; set; } = default!;

        /// <summary>
        /// CPU real-time runtime
        /// </summary>
        [JsonPropertyName("CpuRealtimeRuntime")]
        public long CPURealtimeRuntime { get; set; } = default!;

        /// <summary>
        /// CpusetCpus 0-2, 0,1
        /// </summary>
        [JsonPropertyName("CpusetCpus")]
        public string CpusetCpus { get; set; } = default!;

        /// <summary>
        /// CpusetMems 0-2, 0,1
        /// </summary>
        [JsonPropertyName("CpusetMems")]
        public string CpusetMems { get; set; } = default!;

        /// <summary>
        /// List of devices to map inside the container
        /// </summary>
        [JsonPropertyName("Devices")]
        public IList<DeviceMapping> Devices { get; set; } = default!;

        /// <summary>
        /// List of rule to be added to the device cgroup
        /// </summary>
        [JsonPropertyName("DeviceCgroupRules")]
        public IList<string> DeviceCgroupRules { get; set; } = default!;

        /// <summary>
        /// List of device requests for device drivers
        /// </summary>
        [JsonPropertyName("DeviceRequests")]
        public IList<DeviceRequest> DeviceRequests { get; set; } = default!;

        /// <summary>
        /// Memory soft limit (in bytes)
        /// </summary>
        [JsonPropertyName("MemoryReservation")]
        public long MemoryReservation { get; set; } = default!;

        /// <summary>
        /// Total memory usage (memory + swap); set `-1` to enable unlimited swap
        /// </summary>
        [JsonPropertyName("MemorySwap")]
        public long MemorySwap { get; set; } = default!;

        /// <summary>
        /// Tuning container memory swappiness behaviour
        /// </summary>
        [JsonPropertyName("MemorySwappiness")]
        public long? MemorySwappiness { get; set; }

        /// <summary>
        /// Whether to disable OOM Killer or not
        /// </summary>
        [JsonPropertyName("OomKillDisable")]
        public bool? OomKillDisable { get; set; }

        /// <summary>
        /// Setting PIDs limit for a container; Set `0` or `-1` for unlimited, or `null` to not change.
        /// </summary>
        [JsonPropertyName("PidsLimit")]
        public long? PidsLimit { get; set; }

        /// <summary>
        /// List of ulimits to be set in the container
        /// </summary>
        [JsonPropertyName("Ulimits")]
        public IList<Ulimit> Ulimits { get; set; } = default!;

        /// <summary>
        /// Applicable to Windows
        /// </summary>
        [JsonPropertyName("CpuCount")]
        public long CPUCount { get; set; } = default!;

        /// <summary>
        /// CPU percent
        /// </summary>
        [JsonPropertyName("CpuPercent")]
        public long CPUPercent { get; set; } = default!;

        /// <summary>
        /// Maximum IOps for the container system drive
        /// </summary>
        [JsonPropertyName("IOMaximumIOps")]
        public ulong IOMaximumIOps { get; set; } = default!;

        /// <summary>
        /// Maximum IO in bytes per second for the container system drive
        /// </summary>
        [JsonPropertyName("IOMaximumBandwidth")]
        public ulong IOMaximumBandwidth { get; set; } = default!;

        /// <summary>
        /// Mounts specs used by the container
        /// </summary>
        [JsonPropertyName("Mounts")]
        public IList<Mount>? Mounts { get; set; }

        /// <summary>
        /// MaskedPaths is the list of paths to be masked inside the container (this overrides the default set of paths)
        /// </summary>
        [JsonPropertyName("MaskedPaths")]
        public IList<string> MaskedPaths { get; set; } = default!;

        /// <summary>
        /// ReadonlyPaths is the list of paths to be set as read-only inside the container (this overrides the default set of paths)
        /// </summary>
        [JsonPropertyName("ReadonlyPaths")]
        public IList<string> ReadonlyPaths { get; set; } = default!;

        /// <summary>
        /// Run a custom init inside the container, if null, use the daemon&apos;s configured settings
        /// </summary>
        [JsonPropertyName("Init")]
        public bool? Init { get; set; }
    }
}
