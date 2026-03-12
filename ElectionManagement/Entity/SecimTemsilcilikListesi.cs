using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimTemsilcilikListesi
    {
        [Key]
        public long Id { get; set; }

        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public DateTime? oy_VERME_BASLANGIC_TARIHI { get; set; }
        public DateTime? oy_VERME_BITIS_TARIHI { get; set; }
        public string? oy_VERME_YERI { get; set; }
        public string? temsilcilik_ADI { get; set; }
        public string? ulke_ADI { get; set; }
    }
}
