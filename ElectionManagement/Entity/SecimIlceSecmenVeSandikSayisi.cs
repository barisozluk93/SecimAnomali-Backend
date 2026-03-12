using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIlceSecmenVeSandikSayisi
    {
        public long Id { get; set; }

        public long? SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public long? cezaevi_SANDIK_SAYISI {  get; set; }
        public long? sandik_SAYISI { get; set; }
        public long? secmen_SAYISI { get; set; }
        public long? seyyar_SANDIK_SAYISI { get; set; }
        public long? toplam_SANDIK_SAYISI { get; set; }
        public string? il_ADI { get; set; }
        public int? il_ID { get; set; }
        public string? ilce_ADI { get; set; }
        public int? ilce_ID { get; set; }
        public long? ilce_KODU { get; set; }
    }
}
