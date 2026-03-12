using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIlAdayOgrenimDurumuDagilimi
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
        public long? ilkokul { get; set; }
        public long? ortaokul_LISE { get; set; }
        public long? universite_YUKSEKOKUL { get; set; }
    }
}
