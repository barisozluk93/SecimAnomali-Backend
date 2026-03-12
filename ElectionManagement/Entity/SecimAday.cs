using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimAday
    {
        [Key]
        public long Id { get; set; }
        public long? aday_SIRA_NO { get; set; }
        public string? adaylik_TURU {  get; set; }
        public string? adi_SOYADI { get; set; }
        public string? belde_ADI { get; set; }
        public string? il_ADI { get; set; }
        public int? il_KODU { get; set; }
        public string? ilce_ADI { get; set; }
        public int? ilce_KODU { get; set; }
        public string? parti_ADI { get; set; }
        public string? parti_KISA_ADI { get; set; }
        public string? parti_RENK { get; set; }
        public long? secilme_SIRASI { get; set; }
        public string? secilme_TURU { get; set; }
        public string? secim_CEVRESI_ADI { get; set; }
        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
    }
}
