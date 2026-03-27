#nullable enable
namespace Docker.DotNet.Models
{
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

        [JsonPropertyName("Binds")]
        public IList<string> Binds { get; set; } = default!;

        [JsonPropertyName("ContainerIDFile")]
        public string ContainerIDFile { get; set; } = default!;

        [JsonPropertyName("LogConfig")]
        public LogConfig LogConfig { get; set; } = default!;

        [JsonPropertyName("NetworkMode")]
        public string NetworkMode { get; set; } = default!;

        [JsonPropertyName("PortBindings")]
        public IDictionary<string, IList<PortBinding>> PortBindings { get; set; } = default!;

        [JsonPropertyName("RestartPolicy")]
        public RestartPolicy RestartPolicy { get; set; } = default!;

        [JsonPropertyName("AutoRemove")]
        public bool AutoRemove { get; set; } = default!;

        [JsonPropertyName("VolumeDriver")]
        public string VolumeDriver { get; set; } = default!;

        [JsonPropertyName("VolumesFrom")]
        public IList<string> VolumesFrom { get; set; } = default!;

        [JsonPropertyName("ConsoleSize")]
        [JsonConverter(typeof(ConsoleSizeConverter))]
        public ConsoleSize ConsoleSize { get; set; } = default!;

        [JsonPropertyName("Annotations")]
        public IDictionary<string, string>? Annotations { get; set; }

        [JsonPropertyName("CapAdd")]
        public IList<string> CapAdd { get; set; } = default!;

        [JsonPropertyName("CapDrop")]
        public IList<string> CapDrop { get; set; } = default!;

        [JsonPropertyName("CgroupnsMode")]
        public string CgroupnsMode { get; set; } = default!;

        [JsonPropertyName("Dns")]
        public IList<string> DNS { get; set; } = default!;

        [JsonPropertyName("DnsOptions")]
        public IList<string> DNSOptions { get; set; } = default!;

        [JsonPropertyName("DnsSearch")]
        public IList<string> DNSSearch { get; set; } = default!;

        [JsonPropertyName("ExtraHosts")]
        public IList<string> ExtraHosts { get; set; } = default!;

        [JsonPropertyName("GroupAdd")]
        public IList<string> GroupAdd { get; set; } = default!;

        [JsonPropertyName("IpcMode")]
        public string IpcMode { get; set; } = default!;

        [JsonPropertyName("Cgroup")]
        public string Cgroup { get; set; } = default!;

        [JsonPropertyName("Links")]
        public IList<string> Links { get; set; } = default!;

        [JsonPropertyName("OomScoreAdj")]
        public long OomScoreAdj { get; set; } = default!;

        [JsonPropertyName("PidMode")]
        public string PidMode { get; set; } = default!;

        [JsonPropertyName("Privileged")]
        public bool Privileged { get; set; } = default!;

        [JsonPropertyName("PublishAllPorts")]
        public bool PublishAllPorts { get; set; } = default!;

        [JsonPropertyName("ReadonlyRootfs")]
        public bool ReadonlyRootfs { get; set; } = default!;

        [JsonPropertyName("SecurityOpt")]
        public IList<string> SecurityOpt { get; set; } = default!;

        [JsonPropertyName("StorageOpt")]
        public IDictionary<string, string>? StorageOpt { get; set; }

        [JsonPropertyName("Tmpfs")]
        public IDictionary<string, string>? Tmpfs { get; set; }

        [JsonPropertyName("UTSMode")]
        public string UTSMode { get; set; } = default!;

        [JsonPropertyName("UsernsMode")]
        public string UsernsMode { get; set; } = default!;

        [JsonPropertyName("ShmSize")]
        public long ShmSize { get; set; } = default!;

        [JsonPropertyName("Sysctls")]
        public IDictionary<string, string>? Sysctls { get; set; }

        [JsonPropertyName("Runtime")]
        public string? Runtime { get; set; }

        [JsonPropertyName("Isolation")]
        public string Isolation { get; set; } = default!;

        [JsonPropertyName("CpuShares")]
        public long CPUShares { get; set; } = default!;

        [JsonPropertyName("Memory")]
        public long Memory { get; set; } = default!;

        [JsonPropertyName("NanoCpus")]
        public long NanoCPUs { get; set; } = default!;

        [JsonPropertyName("CgroupParent")]
        public string CgroupParent { get; set; } = default!;

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

        [JsonPropertyName("CpuPeriod")]
        public long CPUPeriod { get; set; } = default!;

        [JsonPropertyName("CpuQuota")]
        public long CPUQuota { get; set; } = default!;

        [JsonPropertyName("CpuRealtimePeriod")]
        public long CPURealtimePeriod { get; set; } = default!;

        [JsonPropertyName("CpuRealtimeRuntime")]
        public long CPURealtimeRuntime { get; set; } = default!;

        [JsonPropertyName("CpusetCpus")]
        public string CpusetCpus { get; set; } = default!;

        [JsonPropertyName("CpusetMems")]
        public string CpusetMems { get; set; } = default!;

        [JsonPropertyName("Devices")]
        public IList<DeviceMapping> Devices { get; set; } = default!;

        [JsonPropertyName("DeviceCgroupRules")]
        public IList<string> DeviceCgroupRules { get; set; } = default!;

        [JsonPropertyName("DeviceRequests")]
        public IList<DeviceRequest> DeviceRequests { get; set; } = default!;

        [JsonPropertyName("MemoryReservation")]
        public long MemoryReservation { get; set; } = default!;

        [JsonPropertyName("MemorySwap")]
        public long MemorySwap { get; set; } = default!;

        [JsonPropertyName("MemorySwappiness")]
        public long? MemorySwappiness { get; set; }

        [JsonPropertyName("OomKillDisable")]
        public bool? OomKillDisable { get; set; }

        [JsonPropertyName("PidsLimit")]
        public long? PidsLimit { get; set; }

        [JsonPropertyName("Ulimits")]
        public IList<Ulimit> Ulimits { get; set; } = default!;

        [JsonPropertyName("CpuCount")]
        public long CPUCount { get; set; } = default!;

        [JsonPropertyName("CpuPercent")]
        public long CPUPercent { get; set; } = default!;

        [JsonPropertyName("IOMaximumIOps")]
        public ulong IOMaximumIOps { get; set; } = default!;

        [JsonPropertyName("IOMaximumBandwidth")]
        public ulong IOMaximumBandwidth { get; set; } = default!;

        [JsonPropertyName("Mounts")]
        public IList<Mount>? Mounts { get; set; }

        [JsonPropertyName("MaskedPaths")]
        public IList<string> MaskedPaths { get; set; } = default!;

        [JsonPropertyName("ReadonlyPaths")]
        public IList<string> ReadonlyPaths { get; set; } = default!;

        [JsonPropertyName("Init")]
        public bool? Init { get; set; }
    }
}
