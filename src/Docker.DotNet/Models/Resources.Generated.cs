#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Resources contains container&apos;s resources (cgroups config, ulimits...)
    /// </summary>
    public class Resources // (container.Resources)
    {
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
    }
}
