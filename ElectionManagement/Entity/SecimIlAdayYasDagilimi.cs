using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIlAdayYasDagilimi
    {
        public long Id { get; set; }

        public long? SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public string? il_ADI { get; set; }
        public int il_ID { get; set; }
        public string? parti_ADI { get; set; }
        public string? parti_KISA_ADI { get; set; }
        public string? parti_RENK { get; set; }
        public long? _18_24 { get; set; }
        public long? _18_24_ORAN { get; set; }
        public long? _25_29 { get; set; }
        public long? _25_29_ORAN { get; set; }
        public long? _30_34 { get; set; }
        public long? _30_34_ORAN { get; set; }
        public long? _35_39 { get; set; }
        public long? _35_39_ORAN { get; set; }
        public long? _40_44 { get; set; }
        public long? _40_44_ORAN { get; set; }
        public long? _45_49 { get; set; }
        public long? _45_49_ORAN { get; set; }
        public long? _50_54 { get; set; }
        public long? _50_54_ORAN { get; set; }
        public long? _55_59 { get; set; }
        public long? _55_59_ORAN { get; set; }
        public long? _60_64 { get; set; }
        public long? _60_64_ORAN { get; set; }
        public long? _65_69 { get; set; }
        public long? _65_69_ORAN { get; set; }
        public long? _70_74 { get; set; }
        public long? _75 { get; set; }
        public long? _75_ORAN { get; set; }
    }
}
