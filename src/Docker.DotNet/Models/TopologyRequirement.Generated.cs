#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TopologyRequirement expresses the user&apos;s requirements for a volume&apos;s
    /// accessible topology.
    /// </summary>
    public class TopologyRequirement // (volume.TopologyRequirement)
    {
        /// <summary>
        /// Requisite specifies a list of Topologies, at least one of which the
        /// volume must be accessible from.
        /// 
        /// Taken verbatim from the CSI Spec:
        /// 
        /// Specifies the list of topologies the provisioned volume MUST be
        /// accessible from.
        /// This field is OPTIONAL. If TopologyRequirement is specified either
        /// requisite or preferred or both MUST be specified.
        /// 
        /// If requisite is specified, the provisioned volume MUST be
        /// accessible from at least one of the requisite topologies.
        /// 
        /// Given
        ///   x = number of topologies provisioned volume is accessible from
        ///   n = number of requisite topologies
        /// The CO MUST ensure n &gt;= 1. The SP MUST ensure x &gt;= 1
        /// If x==n, then the SP MUST make the provisioned volume available to
        /// all topologies from the list of requisite topologies. If it is
        /// unable to do so, the SP MUST fail the CreateVolume call.
        /// For example, if a volume should be accessible from a single zone,
        /// and requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;}
        /// then the provisioned volume MUST be accessible from the &quot;region&quot;
        /// &quot;R1&quot; and the &quot;zone&quot; &quot;Z2&quot;.
        /// Similarly, if a volume should be accessible from two zones, and
        /// requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;}
        /// then the provisioned volume MUST be accessible from the &quot;region&quot;
        /// &quot;R1&quot; and both &quot;zone&quot; &quot;Z2&quot; and &quot;zone&quot; &quot;Z3&quot;.
        /// 
        /// If x&lt;n, then the SP SHALL choose x unique topologies from the list
        /// of requisite topologies. If it is unable to do so, the SP MUST fail
        /// the CreateVolume call.
        /// For example, if a volume should be accessible from a single zone,
        /// and requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;}
        /// then the SP may choose to make the provisioned volume available in
        /// either the &quot;zone&quot; &quot;Z2&quot; or the &quot;zone&quot; &quot;Z3&quot; in the &quot;region&quot; &quot;R1&quot;.
        /// Similarly, if a volume should be accessible from two zones, and
        /// requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z4&quot;}
        /// then the provisioned volume MUST be accessible from any combination
        /// of two unique topologies: e.g. &quot;R1/Z2&quot; and &quot;R1/Z3&quot;, or &quot;R1/Z2&quot; and
        ///  &quot;R1/Z4&quot;, or &quot;R1/Z3&quot; and &quot;R1/Z4&quot;.
        /// 
        /// If x&gt;n, then the SP MUST make the provisioned volume available from
        /// all topologies from the list of requisite topologies and MAY choose
        /// the remaining x-n unique topologies from the list of all possible
        /// topologies. If it is unable to do so, the SP MUST fail the
        /// CreateVolume call.
        /// For example, if a volume should be accessible from two zones, and
        /// requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;}
        /// then the provisioned volume MUST be accessible from the &quot;region&quot;
        /// &quot;R1&quot; and the &quot;zone&quot; &quot;Z2&quot; and the SP may select the second zone
        /// independently, e.g. &quot;R1/Z4&quot;.
        /// </summary>
        [JsonPropertyName("Requisite")]
        public IList<VolumeTopology>? Requisite { get; set; }

        /// <summary>
        /// Preferred is a list of Topologies that the volume should attempt to be
        /// provisioned in.
        /// 
        /// Taken from the CSI spec:
        /// 
        /// Specifies the list of topologies the CO would prefer the volume to
        /// be provisioned in.
        /// 
        /// This field is OPTIONAL. If TopologyRequirement is specified either
        /// requisite or preferred or both MUST be specified.
        /// 
        /// An SP MUST attempt to make the provisioned volume available using
        /// the preferred topologies in order from first to last.
        /// 
        /// If requisite is specified, all topologies in preferred list MUST
        /// also be present in the list of requisite topologies.
        /// 
        /// If the SP is unable to make the provisioned volume available
        /// from any of the preferred topologies, the SP MAY choose a topology
        /// from the list of requisite topologies.
        /// If the list of requisite topologies is not specified, then the SP
        /// MAY choose from the list of all possible topologies.
        /// If the list of requisite topologies is specified and the SP is
        /// unable to make the provisioned volume available from any of the
        /// requisite topologies it MUST fail the CreateVolume call.
        /// 
        /// Example 1:
        /// Given a volume should be accessible from a single zone, and
        /// requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;}
        /// preferred =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;}
        /// then the SP SHOULD first attempt to make the provisioned volume
        /// available from &quot;zone&quot; &quot;Z3&quot; in the &quot;region&quot; &quot;R1&quot; and fall back to
        /// &quot;zone&quot; &quot;Z2&quot; in the &quot;region&quot; &quot;R1&quot; if that is not possible.
        /// 
        /// Example 2:
        /// Given a volume should be accessible from a single zone, and
        /// requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z4&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z5&quot;}
        /// preferred =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z4&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;}
        /// then the SP SHOULD first attempt to make the provisioned volume
        /// accessible from &quot;zone&quot; &quot;Z4&quot; in the &quot;region&quot; &quot;R1&quot; and fall back to
        /// &quot;zone&quot; &quot;Z2&quot; in the &quot;region&quot; &quot;R1&quot; if that is not possible. If that
        /// is not possible, the SP may choose between either the &quot;zone&quot;
        /// &quot;Z3&quot; or &quot;Z5&quot; in the &quot;region&quot; &quot;R1&quot;.
        /// 
        /// Example 3:
        /// Given a volume should be accessible from TWO zones (because an
        /// opaque parameter in CreateVolumeRequest, for example, specifies
        /// the volume is accessible from two zones, aka synchronously
        /// replicated), and
        /// requisite =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z2&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z4&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z5&quot;}
        /// preferred =
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z5&quot;},
        ///   {&quot;region&quot;: &quot;R1&quot;, &quot;zone&quot;: &quot;Z3&quot;}
        /// then the SP SHOULD first attempt to make the provisioned volume
        /// accessible from the combination of the two &quot;zones&quot; &quot;Z5&quot; and &quot;Z3&quot; in
        /// the &quot;region&quot; &quot;R1&quot;. If that&apos;s not possible, it should fall back to
        /// a combination of &quot;Z5&quot; and other possibilities from the list of
        /// requisite. If that&apos;s not possible, it should fall back  to a
        /// combination of &quot;Z3&quot; and other possibilities from the list of
        /// requisite. If that&apos;s not possible, it should fall back  to a
        /// combination of other possibilities from the list of requisite.
        /// </summary>
        [JsonPropertyName("Preferred")]
        public IList<VolumeTopology>? Preferred { get; set; }
    }
}
