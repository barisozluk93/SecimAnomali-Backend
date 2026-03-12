using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimGumrukListesi
    {
        [Key]
        public long Id { get; set; }

        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public string? gumruk_ADI { get; set; }
        public string? il_ADI { get; set; }
        public int? il_ID { get; set; }
        public string? ilce_ADI { get; set; }

        public DateTime? oy_VERME_BASLANGIC_TARIHI { get; set; }
        public DateTime? oy_VERME_BITIS_TARIHI { get; set; }
    }
}
