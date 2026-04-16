#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerUpdateParameters // (main.ContainerUpdateParameters)
    {
        public ContainerUpdateParameters()
        {
        }

        public ContainerUpdateParameters(UpdateConfig UpdateConfig)
        {
            if (UpdateConfig != null)
            {
                this.CPUShares = UpdateConfig.CPUShares;
                this.Memory = UpdateConfig.Memory;
                this.NanoCPUs = UpdateConfig.NanoCPUs;
                this.CgroupParent = UpdateConfig.CgroupParent;
                this.BlkioWeight = UpdateConfig.BlkioWeight;
                this.BlkioWeightDevice = UpdateConfig.BlkioWeightDevice;
                this.BlkioDeviceReadBps = UpdateConfig.BlkioDeviceReadBps;
                this.BlkioDeviceWriteBps = UpdateConfig.BlkioDeviceWriteBps;
                this.BlkioDeviceReadIOps = UpdateConfig.BlkioDeviceReadIOps;
                this.BlkioDeviceWriteIOps = UpdateConfig.BlkioDeviceWriteIOps;
                this.CPUPeriod = UpdateConfig.CPUPeriod;
                this.CPUQuota = UpdateConfig.CPUQuota;
                this.CPURealtimePeriod = UpdateConfig.CPURealtimePeriod;
                this.CPURealtimeRuntime = UpdateConfig.CPURealtimeRuntime;
                this.CpusetCpus = UpdateConfig.CpusetCpus;
                this.CpusetMems = UpdateConfig.CpusetMems;
                this.Devices = UpdateConfig.Devices;
                this.DeviceCgroupRules = UpdateConfig.DeviceCgroupRules;
                this.DeviceRequests = UpdateConfig.DeviceRequests;
                this.MemoryReservation = UpdateConfig.MemoryReservation;
                this.MemorySwap = UpdateConfig.MemorySwap;
                this.MemorySwappiness = UpdateConfig.MemorySwappiness;
                this.OomKillDisable = UpdateConfig.OomKillDisable;
                this.PidsLimit = UpdateConfig.PidsLimit;
                this.Ulimits = UpdateConfig.Ulimits;
                this.CPUCount = UpdateConfig.CPUCount;
                this.CPUPercent = UpdateConfig.CPUPercent;
                this.IOMaximumIOps = UpdateConfig.IOMaximumIOps;
                this.IOMaximumBandwidth = UpdateConfig.IOMaximumBandwidth;
                this.RestartPolicy = UpdateConfig.RestartPolicy;
            }
        }

        /// <summary>
        /// Applicable to all platforms
        /// </summary>
        [JsonPropertyName("CpuShares")]
        public long CPUShares { get; set; } = default!;

        [JsonPropertyName("Memory")]
        public long Memory { get; set; } = default!;

        [JsonPropertyName("NanoCpus")]
        public long NanoCPUs { get; set; } = default!;

        /// <summary>
        /// Applicable to UNIX platforms
        /// </summary>
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

        /// <summary>
        /// Applicable to Windows
        /// </summary>
        [JsonPropertyName("CpuCount")]
        public long CPUCount { get; set; } = default!;

        [JsonPropertyName("CpuPercent")]
        public long CPUPercent { get; set; } = default!;

        [JsonPropertyName("IOMaximumIOps")]
        public ulong IOMaximumIOps { get; set; } = default!;

        [JsonPropertyName("IOMaximumBandwidth")]
        public ulong IOMaximumBandwidth { get; set; } = default!;

        [JsonPropertyName("RestartPolicy")]
        public RestartPolicy RestartPolicy { get; set; } = default!;
    }
}
